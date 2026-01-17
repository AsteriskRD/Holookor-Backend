using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;

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
        {
            try
            {
                await _context.EmailVerifications.AddAsync(verification);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error occurred while creating EmailVerification entry.", ex);
            }
        }

        public async Task<EmailVerification?> GetValidCode(string userProfileId, string code)
        {
            try
            {
                return await _context.EmailVerifications
                    .FirstOrDefaultAsync(v =>
                        v.UserProfileId == userProfileId &&
                        v.Code == code &&
                        !v.IsUsed &&
                        v.ExpiresAt > DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error occurred while retrieving EmailVerification code.", ex);
            }
        }

        public async Task InvalidateExisting(string userProfileId)
        {
            try
            {
                var existing = await _context.EmailVerifications
                    .Where(v => v.UserProfileId == userProfileId && !v.IsUsed)
                    .ToListAsync();

                foreach (var v in existing)
                    v.MarkUsed();
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error occurred while invalidating existing EmailVerification codes.", ex);
            }
        }

        public async Task DeleteExpired(DateTime olderThan)
        {
            try
            {
                var expired = await _context.EmailVerifications
                    .Where(v => v.ExpiresAt < olderThan || v.IsUsed)
                    .ToListAsync();

                _context.EmailVerifications.RemoveRange(expired);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error occurred while deleting expired EmailVerification records.", ex);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error occurred while saving changes to the database.", ex);
            }
        }
    }

}
