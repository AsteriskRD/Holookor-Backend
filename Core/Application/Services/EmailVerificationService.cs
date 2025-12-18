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

        public async Task SendCodeAsync(UserProfile profile)
        {
            await _repo.InvalidateExisting(profile.Id);

            var code = Random.Shared.Next(100000, 999999).ToString();
            var verification = new EmailVerification(profile.Id, code);

            await _repo.CreateAsync(verification);
            await _repo.SaveAsync();

            await _mail.SendAsync(new MailData
            {
                EmailToId = profile.Users!.Email,
                EmailToName = profile.FirstName,
                EmailSubject = "Verify your email",
                EmailBody = $"<h3>Your verification code: {code}</h3>"
            });
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
