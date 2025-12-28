using HolookorBackend.Infrastructure.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(MailData mailData)
    {
        var apiKey = _configuration["SENDGRID_API_KEY"];
        var fromEmail = _configuration["SENDGRID_FROM_EMAIL"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(fromEmail))
        {
            throw new Exception("SendGrid configuration is missing");
        }

        var client = new SendGridClient(apiKey);

        var from = new EmailAddress(fromEmail, "Holookor");
        var to = new EmailAddress(mailData.EmailToId, mailData.EmailToName);

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            mailData.EmailSubject,
            plainTextContent: null,
            htmlContent: mailData.EmailBody
        );

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Body.ReadAsStringAsync();
            throw new Exception($"SendGrid failed: {body}");
        }
    }
}
