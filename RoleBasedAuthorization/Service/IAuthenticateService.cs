using RoleBasedAuthorization.DTOs.Requests;
using RoleBasedAuthorization.Model;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Service
{
    public interface IAuthenticateService
    {
        User Login(string username, string password);

        User GenerateJwtToken(User user);

        string GenerateRefreshToken(User user);

        //bool VerifyToken(TokenRequest tokenRequest);

        //bool ValidateToken(TokenRequest tokenRequest);
    }
}
