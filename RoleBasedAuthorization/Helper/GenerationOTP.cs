using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Helper
{
    public class GenerationOTP
    {

        // generation random otp
        public static int GenerationRandomOTP()
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
