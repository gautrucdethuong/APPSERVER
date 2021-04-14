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

        IEnumerable<User> searchUser(string name, string phone);

        IEnumerable<User> filterUserByRole(string role);

        bool CheckExistProperties(string email, string username, string phone);
    }
}
