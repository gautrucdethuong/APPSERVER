using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Model
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int? Quantity { get; set; }

        public string UserId { get; set; }

        public int? ProductId { get; set; }

        public DateTime Request_Date { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
