
namespace Connex.Services.Inteface
{
    public interface IEmailValidationService
    {
        Task<bool> IsEmailValidAsync(string email);
    }
}
