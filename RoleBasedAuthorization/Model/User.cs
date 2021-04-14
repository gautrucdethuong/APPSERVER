using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Model
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [MaxLength(16)]
        [RegularExpression("^[a-z0-9_-]{3,16}$", ErrorMessage = "Username incorrect format.")]
        public string user_username { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Password must be between 4 and 8 digits long and include at least one numeric digit.")]
        public string user_password { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-z A-Z]+$", ErrorMessage = "Fullname incorrect format.")]
        public string user_fullname { get; set; }
       
        [Required]
        [MaxLength(32)]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage ="Email incorrect format.")]
        public string user_email  { get; set; }

        [MaxLength(15)]
        [Required]
        [RegularExpression("^\\+?(0|84)\\d{9}$|^\\d{11}$", ErrorMessage = "Phone incorrect format.")]
        public string user_phone { get; set; }

        public string user_role { get; set; }

        public string user_token { get; set; }

        public string user_refreshToken { get; set; }

        public string user_otp { get; set; }

        [RegularExpression("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", ErrorMessage = "Link url incorrect format.")]
        public string user_avt { get; set; }

        [RegularExpression("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", ErrorMessage = "Link url incorrect format.")]
        public string user_picture { get; set; }

    }
}
