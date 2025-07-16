using Connex.Models;
using Connex.Services.Inteface;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Connex.Services.Implementation
{
    public class QrCodeGenerator : IQrCodeGenerator
    {
        private readonly QrCodeSettings _settings;

        public QrCodeGenerator(IOptions<QrCodeSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateQrCodeUrl(object data)
        {
            var encoded = System.Web.HttpUtility.UrlEncode(JsonSerializer.Serialize(data));
            return $"{_settings.BaseUrl}?size={_settings.Size}&data={encoded}";
        }
    }
}
