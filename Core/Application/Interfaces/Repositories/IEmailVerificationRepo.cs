using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface IEmailVerificationRepo
    {
        Task CreateAsync(EmailVerification verification);
        Task<EmailVerification?> GetValidCode(string userProfileId, string code);
        Task InvalidateExisting(string userProfileId);
        Task DeleteExpired(DateTime olderThan);
        Task SaveAsync();
    }

}
