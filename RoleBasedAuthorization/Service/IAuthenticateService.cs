using RoleBasedAuthorization.Model;

namespace RoleBasedAuthorization.Service
{
    public interface IAuthenticateService
    {
        User Login(string username, string password);
    }
}
