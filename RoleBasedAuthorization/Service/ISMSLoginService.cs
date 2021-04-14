using RoleBasedAuthorization.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Service
{
    public interface ISMSLoginService
    {
        public bool SMSSendOTPLogin(string phone);

        public User VerificationOTPLogin(string otp);
    }
}
