using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.DTOs.Requests;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenManagerController : Controller
    {            
        private readonly IAuthenticateService _authen;
        private readonly DBContext _db;

        public AuthenManagerController(IAuthenticateService authenticate, DBContext dBConext)
        {
            _authen = authenticate;            
            _db = dBConext;
        }
            
        [HttpPost]
        [Route("login")]
        public async Task<JsonResult> Login([FromBody] AuthenticateModel login)
        {
            //check user login
            var user = _authen.Login(login.username, login.password);

            if (user == null)
            {
                return Json(JsonResultResponse.ResponseFail("Username or password incorrect."));
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseSuccess(user));
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<JsonResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            var response = await VerifyToken(tokenRequest);

            if (ModelState.IsValid)
            {
                if (response == null)
                {
                    return response;
                }
            }
            await Task.Delay(1000);
            return response;

        }

        private async Task<JsonResult> VerifyToken(TokenRequest tokenRequest)
        {
            var principal = GetPrincipalFromExpiredToken(tokenRequest.Token);

            string nameUnique = principal.Identity.Name;

            var identityName = _db.Users.SingleOrDefault(u => u.user_username == nameUnique);

            if(identityName == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            try
            {
                // Check the refresh token we got if its saved in the db
                var storedRefreshToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);
                
                //Check the refresh is null
                if (storedRefreshToken == null)
                {
                    return Json(JsonResultResponse.ResponseFail("Refresh token doesn't exist."));
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return Json(JsonResultResponse.ResponseFail("Refresh token has expired, user needs to relogin."));
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return Json(JsonResultResponse.ResponseFail("Refresh token has been used."));
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return Json(JsonResultResponse.ResponseFail("Token has been revoked."));
                }

                storedRefreshToken.IsUsed = true;
                _db.RefreshTokens.Update(storedRefreshToken);
                await _db.SaveChangesAsync();

                var userUpdate = _db.Users.Find(storedRefreshToken.UserId);
                var user = _authen.GenerateJwtToken(userUpdate);

                string refreshtoken = _authen.GenerateRefreshToken(userUpdate);

                return Json(new
                {
                    accessToken = user.user_token,
                    refreshToken = refreshtoken
                });
            }
            catch (Exception)
            {
                throw new SecurityTokenException("Invalid token");
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = Constant.Issuer,
                ValidAudience = Constant.Audiance,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constant.Secret)),
                ValidateLifetime = false,
                RequireExpirationTime = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }                
            return principal;
        }


        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<JsonResult> Revoke([FromBody] TokenRequest tokenRequest)
        {            
            var storedRefreshToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);

            string username = User.Identity.Name;
            var user = _db.Users.SingleOrDefault(u => u.user_username == username);
            if (user == null)
            {
                return Json(JsonResultResponse.ResponseFail("Revoke failed."));
            }
            
            user.user_refreshToken = null;
            user.user_token = null;
            storedRefreshToken.IsRevoked = true;

            await _db.SaveChangesAsync();
            await Task.Delay(500);
           return Json(JsonResultResponse.ResponseChange("Revoke successed."));
        }

    }
    
}
