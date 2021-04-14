using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Model
{
    public class VerifyModel
    {
        [Required]
        public string otp { get; set; }
    }
}
