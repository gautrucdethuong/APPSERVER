using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Model
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        [Required]
        public int Rating { get; set; }

        public DateTime Sent { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
