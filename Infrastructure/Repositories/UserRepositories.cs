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
            try
            {
                return await _context.Users
                    .Include(x => x.UserProfile)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }

        public async Task<ICollection<User>> GetAll(Paging paging)
        {
            try
            {
                return await _context.Users
                    .Include(x => x.UserProfile)
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                
                return new List<User>();
            }
        }

        public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate)
        {
            try
            {
                return await _context.Users
                    .Include(x => x.UserProfile)
                    .SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
