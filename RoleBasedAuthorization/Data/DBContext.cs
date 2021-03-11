using Microsoft.EntityFrameworkCore;
using RoleBasedAuthorization.Model;

namespace RoleBasedAuthorization.Data
{
    public class DBContext: DbContext
    {
        //protected DBContext();

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
    }
}
