using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class UserProfileRepositories : BaseRespositories<UserProfile>, IUserProfileRepo
    {
        private readonly HolookorSystem _context;
        public UserProfileRepositories(HolookorSystem context) : base(context)
        {
            _context = context;
        }

        public async Task<UserProfile?> Get(string id)
        {
            try
            {
                return await _context.UserProfiles
                    .Include(p => p.Users)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving UserProfile by id: {id}.", ex);
            }
        }

        public async Task<ICollection<UserProfile>> GetAll(Paging paging)
        {
            try
            {
                return await _context.UserProfiles
                    .Include(p => p.Users)
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving UserProfiles with paging.", ex);
            }
        }

        public async Task<UserProfile?> GetAsync(Expression<Func<UserProfile, bool>> predicate)
        {
            try
            {
                return await _context.UserProfiles
                    .Include(p => p.Users)
                    .SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving UserProfile by predicate.", ex);
            }
        }
    }
}
