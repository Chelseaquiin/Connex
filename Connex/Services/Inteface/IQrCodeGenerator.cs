namespace Connex.Services.Inteface
{
    public interface IQrCodeGenerator
    {
        string GenerateQrCodeUrl(object data);
    }
}
