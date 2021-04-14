using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Service;
using System;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace RoleBasedAuthorization.Reponsitory
{
    public class OTPSenderReponsitory : IOTPSenderService
    {
        private DBContext db;

        public OTPSenderReponsitory(DBContext dBContext)
        {
            db = dBContext;
        }


        public bool OTPSenderUser(string phone)
        {
            
            //Account SID and Auth Token 
            const string accountSid = "AC10b661551dca73f562a3a047523b3217";
            const string authToken = "b6cf00ac221fa7b4a203d205ef250396";
           
            TwilioClient.Init(accountSid, authToken);
            int otp = GenerationOTP();
            // check phone exist in db
            var entity = db.Users.FirstOrDefault(item => item.user_phone == phone);

                if(entity != null)
                {
                    entity.user_otp = otp.ToString();
                    db.SaveChanges();

                // create new password send user
                // Send a text message
                    MessageResource.Create(
                        body: otp.ToString() + " (ma OTP Application se het han sau 5 phut). Luu y: Tuyet doi khong cung cap ma OTP cua ban vi bat cu ly do gi.",
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


        // verify otp sended
        public bool VerificationOTP(string otp)
        {
            //Account SID and Auth Token at twilio
            const string accountSid = "AC10b661551dca73f562a3a047523b3217";
            const string authToken = "b6cf00ac221fa7b4a203d205ef250396";

            TwilioClient.Init(accountSid, authToken);
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


        //random password is 8 character  
        private string CreateRandomPassword(int length = 8)
        {
            // Create a string of characters, numbers that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
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
