using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Model
{
    public class VerifyModel
    {

        public string otp { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}
