using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Model
{
    public class OTPModel
    {
        [Required]
        public string phone { get; set; }
    }
}
