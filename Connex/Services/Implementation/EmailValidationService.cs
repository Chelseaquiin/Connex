using Connex.Models;
using Connex.Services.Inteface;
using Microsoft.Extensions.Options;

namespace Connex.Services.Implementation
{
    public class EmailValidationService : IEmailValidationService
    {
            private readonly HttpClient _httpClient;
        private readonly KickboxSettings _settings;

        public EmailValidationService(HttpClient httpClient, IOptions<KickboxSettings> options)
            {
                _httpClient = httpClient;
               _settings = options.Value;
        }

            public async Task<bool> IsEmailValidAsync(string email)
            {
            var url = $"{_settings.BaseUrl}/disposable/{email}";
            var response = await _httpClient.GetFromJsonAsync<KickboxResponse>(url);
                return response != null && response.Disposable == false;
            }

            private class KickboxResponse
            {
                public bool Disposable { get; set; }
            }
        }

    }

