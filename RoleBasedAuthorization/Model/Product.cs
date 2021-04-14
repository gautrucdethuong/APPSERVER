using System.ComponentModel.DataAnnotations;


namespace RoleBasedAuthorization.Model
{
    public class Product
    {
        [Key]
        public int product_id { set; get; }

        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Name incorrect format.")]
        public string product_name { set; get; }

        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Origin name incorrect format.")]
        public string product_origin { set; get; }

        [Required]
        [RegularExpression("^\\d+(,\\d{3})*(\\.\\d{1,2})?$", ErrorMessage = "Price incorrect format.")]
        public double product_price { set; get; }

        [Required]
        public string product_description { set; get; }

        [Required]
        [RegularExpression("^[A-Z]$", ErrorMessage = "Size incorrect format.")]
        public string product_size { set; get; }

        public string product_rating { set; get; }

        [RegularExpression("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", ErrorMessage = "Link url incorrect format.")]
        public string product_image { set; get; }
    }
}
