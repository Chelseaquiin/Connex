namespace Connex.Models
{
    public class KickboxSettings
    {
        public string BaseUrl { get; set; } = default!;
    }

    public class SmtpSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string From { get; set; } = default!;
    }

    public class QrCodeSettings
    {
        public string BaseUrl { get; set; } = default!;
        public string Size { get; set; } = "200x200";
    }

    public class JwtSettings
    {
        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpiresInMinutes { get; set; }
    }


}
