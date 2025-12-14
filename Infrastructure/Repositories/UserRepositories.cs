using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class UserRepositories : BaseRespositories<User>, IUserRepo
    {
        private readonly HolookorSystem _context;
        public UserRepositories(HolookorSystem context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> Get(string id)
        {
            return await _context.Users
                  .Include(x => x.UserProfile)
               .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<User>> GetAll(Paging paging)
        {
            return await _context.Users
                .Include(x => x.UserProfile)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();
        }

        public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users
                  .Include(x => x.UserProfile)
                .SingleOrDefaultAsync(predicate);
        }
    }
}
