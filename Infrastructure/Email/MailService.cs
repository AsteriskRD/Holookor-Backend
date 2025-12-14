using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace HolookorBackend.Infrastructure.Email
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(MailData mailData)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = mailData.EmailSubject,
                Body = mailData.EmailBody,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(mailData.EmailToId, mailData.EmailToName));

            using var smtp = new SmtpClient(_settings.Server, _settings.Port)
            {
                Credentials = new NetworkCredential(
                    _settings.UserName,
                    _settings.Password
                ),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
