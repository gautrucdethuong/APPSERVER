using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.DTOs
{
    public class AuthResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string Result { get; set; }

        public List<string> Errors { get; set; }
    }
}
