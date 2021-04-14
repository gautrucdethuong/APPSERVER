using Microsoft.IdentityModel.Tokens;
using OAuth2;
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
            const string accountSid = "AC10b661551dca73f562a3a047523b3217";
            const string authToken = "b6cf00ac221fa7b4a203d205ef250396";

            TwilioClient.Init(accountSid, authToken);

            int otp = GenerationOTP();
            // check phone exist in db
            var entity = db.Users.FirstOrDefault(item => item.user_phone == phone);

            if (entity != null)
            {
                entity.user_otp = otp.ToString();
                db.SaveChanges();

                // Send a sms otp
                MessageResource.Create(
                    body: otp.ToString() + " (ma OTP se het han sau 5 phut). Luu y: Tuyet doi khong cung cap ma OTP cua ban vi bat cu ly do gi.",
                    from: new Twilio.Types.PhoneNumber("+17015994809"),
                    to: new Twilio.Types.PhoneNumber("+840832511369")
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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Constant.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // create claims
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, user.user_fullname),
                        new Claim(ClaimTypes.Email, user.user_email),

                }),

                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // write token
            user.user_token = tokenHandler.WriteToken(token);



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

    }
}
