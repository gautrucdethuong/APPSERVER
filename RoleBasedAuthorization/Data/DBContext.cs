using Microsoft.EntityFrameworkCore;
using RoleBasedAuthorization.Model;


namespace RoleBasedAuthorization.Data
{
    public class DBContext: DbContext
    {
        
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<AuthenticateModel> AuthenticateModel { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Cart> Carts { get; set; }

    }
}
