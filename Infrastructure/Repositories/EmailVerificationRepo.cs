using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class EmailVerificationRepo : IEmailVerificationRepo
    {
        private readonly HolookorSystem _context;

        public EmailVerificationRepo(HolookorSystem context)
        {
            _context = context;
        }

        public async Task CreateAsync(EmailVerification verification)
            => await _context.EmailVerifications.AddAsync(verification);

        public async Task<EmailVerification?> GetValidCode(string userProfileId, string code)
            => await _context.EmailVerifications
                .FirstOrDefaultAsync(v =>
                    v.UserProfileId == userProfileId &&
                    v.Code == code &&
                    !v.IsUsed &&
                    v.ExpiresAt > DateTime.UtcNow);

        public async Task InvalidateExisting(string userProfileId)
        {
            var existing = await _context.EmailVerifications
                .Where(v => v.UserProfileId == userProfileId && !v.IsUsed)
                .ToListAsync();

            foreach (var v in existing)
                v.MarkUsed();
        }

        public async Task DeleteExpired(DateTime olderThan)
        {
            var expired = await _context.EmailVerifications
                .Where(v => v.ExpiresAt < olderThan || v.IsUsed)
                .ToListAsync();

            _context.EmailVerifications.RemoveRange(expired);
        }

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();
    }

}
