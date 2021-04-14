using RoleBasedAuthorization.DTOs.Requests;
using RoleBasedAuthorization.Model;

namespace RoleBasedAuthorization.Service
{
    public interface IAuthenticateService
    {
        User Login(string username, string password);

        User GenerateJwtToken(User user);

        bool ValidateToken(TokenRequest tokenRequest);
    }
}
