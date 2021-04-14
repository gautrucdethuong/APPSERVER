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
            
        [HttpPost("login")]
        public JsonResult Login([FromBody] AuthenticateModel login)
        {
            //check user login
            var user = _authen.Login(login.username, login.password);

            if (user == null)
            {
                return Json(JsonResultResponse.ResponseFail("Username or password incorrect."));
            }                
            return Json(JsonResultResponse.ResponseSuccess(user));
        }

        [HttpPost]
        [Route("RefreshToken")]
        public JsonResult RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            /*if (ModelState.IsValid)
            {
                var res =  VerifyToken(tokenRequest);

                if (res == null)
                {
                    return Json(JsonResultResponse.ResponseFail("Invalid token."));
                }
                return Json(JsonResultResponse.ResponseSuccess(res));
            }
            return Json(JsonResultResponse.ResponseFail("Invalid payload."));*/


            if (ModelState.IsValid)
            {
                var res = _authen.ValidateToken(tokenRequest);
                if(res == false)
                {
                    return Json(JsonResultResponse.ResponseFail("Invalid token."));
                }
                return Json(JsonResultResponse.ResponseSuccess(res));
            }
            return Json(JsonResultResponse.ResponseFail("Invalid payload."));
        }



        private JsonResult VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
               /* var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
                
                

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }*/

                var storedRefreshToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);
                // Check the token we got if its saved in the db
                if (storedRefreshToken == null)
                {
                    return Json(JsonResultResponse.ResponseFail("Refresh token doesn't exist."));
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return Json(JsonResultResponse.ResponseFail("Token has expired, user needs to relogin."));
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return Json(JsonResultResponse.ResponseFail("Token has been used."));
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return Json(JsonResultResponse.ResponseFail("Token has been revoked."));
                }

                storedRefreshToken.IsUsed = true;
                _db.RefreshTokens.Update(storedRefreshToken);
                _db.SaveChanges();

                var userUpdate = _db.Users.Find(storedRefreshToken.UserId);
                var user = _authen.GenerateJwtToken(userUpdate);
                return Json(JsonResultResponse.ResponseSuccess(user));
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            // CHANGE HERE, from .ToLocalTime to .ToUniverstalTime
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
    }
    
}
