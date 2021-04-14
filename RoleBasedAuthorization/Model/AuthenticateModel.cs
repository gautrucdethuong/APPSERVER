using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Model
{
    public class AuthenticateModel
    {
        [Required]
        [Key]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}
