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






        /*if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.username == user.username))
                {
                    return base.Content("Username " + user.username + " is already exist. Please enter a different username.");
                }
                else if (db.Users.Any(x => x.email == user.email))
                {
                    return base.Content("Email " + user.email + " is already exist. Please enter a different email.");
                }
                else if (db.Users.Any(x => x.phone == user.phone))
                {
                    return base.Content("Number phone " + user.phone + " is already exist. Please enter a different number phone.");
                }
            }*/
    }
}