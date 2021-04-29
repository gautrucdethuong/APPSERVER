using Microsoft.IdentityModel.Tokens;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RoleBasedAuthorization.Reponsitory
{
    public class SMSLoginRepository : ISMSLoginService
    {
        private readonly DBContext db;

        public SMSLoginRepository(DBContext dbContext)
        {
            db = dbContext;
        }

        //send opt to user
        public bool SMSSendOTPLogin(string phone)
        {
            //Account SID and Auth Token 
            TwilioClient.Init(Constant.accountSid, Constant.authToken);

            int otp = GenerationOTP();
            // check phone exist in db
            var entity = db.Users.FirstOrDefault(item => item.user_phone == phone);

            if (entity != null)
            {
                entity.user_otp = otp.ToString();
                db.SaveChanges();

                // Send a sms otp
                MessageResource.Create(
                    body: otp.ToString() + Constant.SMSMessageLogin,
                    from: new Twilio.Types.PhoneNumber(Constant.contactSystems),
                    to: new Twilio.Types.PhoneNumber(Constant.contactCustomer)
                );
                return true;
            }

            else
            {
                return false;
            }
        }


        // verify sms sender
        public User VerificationOTPLogin(string otp)
        {

            // check opt exist in database              
            var user = db.Users.FirstOrDefault(item => item.user_otp == otp);

            if (user == null)
            {
                return null;
            }
            // authentication successfully so generate jwt token
            GenerateJwtToken(user);
            GenerateRefreshToken(user);

            return user;
        }


        // generation random otp
        private int GenerationOTP()
        {
            try
            {
                int min = 100000;
                int max = 999999;
                int otp = 0;

                Random rdm = new Random();
                otp = rdm.Next(min, max);
                return otp;

            }
            catch (Exception)
            {
                throw new Exception("Generation OTP failed.");
            }
        }


        private User GenerateJwtToken(User user)
        {
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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                // the life span of the token needs to be shorter and utilise refresh token to keep the user signedin
                Expires = DateTime.UtcNow.AddMinutes(15),

                // here we are adding the encryption alogorithim information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Audience = Constant.Audiance,
                Issuer = Constant.Issuer
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            user.user_token = jwtTokenHandler.WriteToken(token);
            user.user_exprires_at = (DateTime)tokenDescriptor.Expires;

            return user;
        }

        //generation refresh token
        private string GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken()
            {
                JwtId = Guid.NewGuid().ToString(),
                IsUsed = false,
                IsRevoked = false,
                UserId = user.user_id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                Token = RandomStringRefresh(35) + Guid.NewGuid(),

            };
            user.user_refreshToken = refreshToken.Token;
            user.user_refresh_token_expires_at = refreshToken.ExpiryDate;

            db.RefreshTokens.Add(refreshToken);
            db.SaveChanges();
            return user.user_refreshToken;
        }

        // generation random string refresh
        private string RandomStringRefresh(int lenght)
        {
            var random = new Random();
            var chars = Constant.randomString;
            return new string(Enumerable.Repeat(chars, lenght).Select(x => x[random.Next(x.Length)]).ToArray());
        }

    }
}
