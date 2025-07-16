
namespace Connex.Services.Inteface
{
    public interface IEmailSender
    {
        Task SendInvitationEmailAsync(string to, string subject, string htmlBody);
    }
}
