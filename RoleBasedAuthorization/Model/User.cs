using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Model
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9_-]{3,16}$", ErrorMessage = "Username at least 3 characters.")]
        //[IsValidData(ErrorMessage = "The Email address already exists.")]
        public string username { get; set; }

        [Required]       
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Password must be between 4 and 8 digits long and include at least one numeric digit.")]
        public string password { get; set; }

        [Required]
        [RegularExpression("^[a-z A-Z]+$", ErrorMessage = "Fullname at least 2 words.")]
        public string fullname { get; set; }
       
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage ="Email incorrect format.")]
        public string email  { get; set; }

        [MaxLength(15)]
        [Required]
        [RegularExpression("^\\d{10}$|^\\d{11}$", ErrorMessage = "Phone incorrect format.")]
        public string phone { get; set; }

        public string role { get; set; }

        public string Token { get; set; }

    }
}
