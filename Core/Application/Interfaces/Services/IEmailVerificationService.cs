using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Interfaces.Services
{
    public interface IEmailVerificationService
    {
        Task<bool> ConfirmCode(string userProfileId, string code);
            Task SendCodeAsync(string profileId, string email, string firstName);
        
    }
}
