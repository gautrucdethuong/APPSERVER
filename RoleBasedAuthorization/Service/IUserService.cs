using RoleBasedAuthorization.Model;
using System.Collections.Generic;


namespace RoleBasedAuthorization.Service
{
    public interface IUserService
    {
        IEnumerable<User> getAllUser();

        User GetUser(int id);

        User CreateUser(User user);

        User EditUser(User user);

        void DeleteUser(User user);

        object CheckExistProperties(string email, string username, string phone);
    }
}
