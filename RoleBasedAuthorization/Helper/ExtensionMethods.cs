using System.Collections.Generic;
using System.Linq;
using RoleBasedAuthorization.Model;

namespace RoleBasedAuthorization.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users) 
        {
            if (users == null) return null;

            return users.Select(x => x.WithoutPassword());
        }

        //get user by token
        public static User WithoutPassword(this User user) 
        {
            if (user == null) return null;

            user.password = null;
            return user;
        }






        
    }
}