using RoleBasedAuthorization.Model;


namespace RoleBasedAuthorization.Service
{
    public interface IEmailSenderService
    {
        User EmailSender(User user);
    }
}
