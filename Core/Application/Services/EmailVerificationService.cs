using HolookorBackend.Core.Application.Exceptions.HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Email;

namespace HolookorBackend.Core.Application.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailVerificationRepo _repo;
        private readonly IUserProfileRepo _profileRepo;
        private readonly IMailService _mail;

        public EmailVerificationService(
            IEmailVerificationRepo repo,
            IUserProfileRepo profileRepo,
            IMailService mail)
        {
            _repo = repo;
            _profileRepo = profileRepo;
            _mail = mail;
        }

        public async Task SendCodeAsync(string profileId, string email, string firstName)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email address not found in token");

            await _repo.InvalidateExisting(profileId);

            var code = Random.Shared.Next(100000, 999999).ToString();
            var verification = new EmailVerification(profileId, code);

            await _repo.CreateAsync(verification);
            await _repo.SaveAsync();


            try
            {
                await _mail.SendAsync(new MailData
                {
                    EmailToId = email,
                    EmailToName = firstName,
                    EmailSubject = "Verify your email",
                    EmailBody = $"<h3>Your verification code: {code}</h3>"
                });
            }
            catch (Exception ex)
            {
                throw new DomainException($"Failed to send verification email: {ex.Message}");
            }
        }

        public async Task<bool> ConfirmCode(string userProfileId, string code)
        {
            var verification = await _repo.GetValidCode(userProfileId, code);
            if (verification == null) return false;

            verification.MarkUsed();

            var profile = await _profileRepo.Get(userProfileId);
            profile!.IsEmailVerified = true;

            await _repo.SaveAsync();
            return true;
        }
    }

}
