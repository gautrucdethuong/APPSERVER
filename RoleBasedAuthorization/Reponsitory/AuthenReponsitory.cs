using Microsoft.IdentityModel.Tokens;
using OAuth2;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.DTOs.Requests;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;


namespace RoleBasedAuthorization.Reponsitory
{
    public class AuthenReponsitory : IAuthenticateService
    {
        private readonly DBContext db;

        public AuthenReponsitory(DBContext dbContext)
        {
            db = dbContext;
        }
        
        // generation token
        public User Login(string username, string password)
        {

            List<User> users = db.Users.ToList();
            var user = users.SingleOrDefault(x => x.user_username == username && x.user_password == password);


            // return null if user not found
            if (user == null)
            {
                return null;
            }

            var jwtToken = GenerateJwtToken(user);

            return user;
            
        }

        public User GenerateJwtToken(User user)
        {
            // Now its ime to define the jwt token which will be responsible of creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the appsettings
            var key = Encoding.UTF8.GetBytes(Constant.Secret);

            // we define our token descriptor
            // so it could contain their id, name, email the good part is that these information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.user_id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.user_username),
                new Claim(JwtRegisteredClaimNames.Email, user.user_email),
                // the JTI is used for our refresh token which we will be convering in the next video
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
                // the life span of the token needs to be shorter and utilise refresh token to keep the user signedin
                Expires = DateTime.UtcNow.AddMinutes(1),

                // here we are adding the encryption alogorithim information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            user.user_token = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.user_id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                Token = RandomStringRefresh(35)+ Guid.NewGuid(),

            };
            user.user_refreshToken = refreshToken.Token;

            db.RefreshTokens.Add(refreshToken);
            db.SaveChanges();

            return user;
        }

        private string RandomStringRefresh(int lenght)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, lenght).Select(x => x[random.Next(x.Length)]).ToArray());
        }


        public bool ValidateToken(TokenRequest tokenRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters(tokenRequest.Token);

            SecurityToken validatedToken;

            IPrincipal principal = tokenHandler.ValidateToken(tokenRequest.Token, validationParameters, out validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    return false;
                }

                var storedRefreshToken = db.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);
                // Check the token we got if its saved in the db
                if (storedRefreshToken == null)
                {
                    return false;
                    //return Json(JsonResultResponse.ResponseFail("Refresh token doesn't exist."));
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return false;
                    //return Json(JsonResultResponse.ResponseFail("Token has expired, user needs to relogin."));
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return false;
                    //return Json(JsonResultResponse.ResponseFail("Token has been used."));
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return false;
                    //return Json(JsonResultResponse.ResponseFail("Token has been revoked."));
                }

                storedRefreshToken.IsUsed = true;
                db.RefreshTokens.Update(storedRefreshToken);
                db.SaveChanges();

                var userUpdate = db.Users.Find(storedRefreshToken.UserId);
                var user = GenerateJwtToken(userUpdate);
                //return Json(JsonResultResponse.ResponseSuccess(user));
            }

            return true;
        }

        private TokenValidationParameters GetValidationParameters(String token)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constant.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                RequireExpirationTime = false,

                // Allow to use seconds for expiration of token
                // Required only when token lifetime less than 5 minutes
                // THIS ONE
            //    ClockSkew = TimeSpan.Zero // The same key as the one that generate the token
            };
        }
    }
}
