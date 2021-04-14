
namespace RoleBasedAuthorization.Service
{
    public interface IOTPSenderService
    {
        bool OTPSenderUser(string phone);

        bool VerificationOTP(string otp);

    }
}
