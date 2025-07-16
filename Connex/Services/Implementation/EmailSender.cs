using Connex.Models;
using Connex.Services.Inteface;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace Connex.Services.Implementation
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;

        public EmailSender(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
        }


        public async Task SendInvitationEmailAsync(string to, string subject, string htmlBody)
        {
            try
            {
                using var client = new SmtpClient(_settings.Host, _settings.Port)
                {
                    EnableSsl = _settings.EnableSsl,
                    Credentials = new NetworkCredential(_settings.Username, _settings.Password)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_settings.From),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                message.To.Add(to);
                try
                {
                    await client.SendMailAsync(message);

                }
                catch (SmtpException smtpEx)
                {
                    Console.WriteLine($"SMTP ERROR: {smtpEx.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GENERIC ERROR: {ex.Message}");
                throw;
            }
        }
    }
}
