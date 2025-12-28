namespace HolookorBackend.Infrastructure.Email
{
    public interface IMailService
    {
        Task SendAsync(MailData mailData);
    }

}
