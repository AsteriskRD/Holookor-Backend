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
            return await _context.UserProfiles
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<UserProfile>> GetAll(Paging paging)
        {
            return await _context.UserProfiles
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();
        }

        public async Task<UserProfile?> GetAsync(Expression<Func<UserProfile, bool>> predicate)
        {
            return await _context.UserProfiles
                .SingleOrDefaultAsync(predicate);
        }
    }
}
