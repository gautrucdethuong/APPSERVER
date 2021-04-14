using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Model
{
    public class Cart
    {
        [Key]
        public int cart_id { set; get; }

        [ForeignKey("Product")]
        public int product_id { set; get; }

        [Required]
        public int quantity { set; get; }
       
    }
}
