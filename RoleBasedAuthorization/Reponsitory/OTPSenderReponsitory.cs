using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Service;
using System;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace RoleBasedAuthorization.Reponsitory
{
    public class OTPSenderReponsitory : IOTPSenderService
    {
        private readonly DBContext db;

        public OTPSenderReponsitory(DBContext dBContext)
        {
            db = dBContext;
        }

        public bool OTPSenderUser(string phone)
        {
            
            TwilioClient.Init(Constant.accountSid, Constant.authToken);

            int otp = GenerationOTP.GenerationRandomOTP();
            // check phone exist in db
            var entity = db.Users.FirstOrDefault(item => item.user_phone == phone);

                if(entity != null)
                {
                    entity.user_otp = otp.ToString();
                    db.SaveChanges();

                // create new password send user
                // Send a text message
                    MessageResource.Create(
                        body: otp.ToString() + Constant.OPTMessage,
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


        // verify otp sended
        public bool VerificationOTP(string otp)
        {
            //Account SID and Auth Token at twilio
            TwilioClient.Init(Constant.accountSid, Constant.authToken);

            string randompassword = CreateRandomPassword();

            // check opt exist in database              
                var entity = db.Users.FirstOrDefault(item => item.user_otp == otp);
                if (entity != null)
                {
                    // update password when request user
                    entity.user_password = randompassword;
                    db.SaveChanges();

                    // create new password send user
                    // Send a text message
                    MessageResource.Create(
                        body: "Your new password is: " + randompassword.ToString(),
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

        //random password is 8 character  
        private string CreateRandomPassword(int length = 8)
        {
            // Create a string of characters, numbers that allowed in the password  
            string validChars = Constant.randomString;
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
    
    }
}
