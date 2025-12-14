using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class ParentRepositories : BaseRespositories<Parent>, IParentRepo
    {
        private readonly HolookorSystem _context;
        public ParentRepositories(HolookorSystem context) : base(context)
        {
            _context = context;
        }

        public async Task<Parent?> Get(string id)
        {
            return await _context.Parents
                .Include(x => x.Profile)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Parent>> GetAll(Paging paging)
        {
            return await _context.Parents
                .Include(x => x.Profile)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();
        }

        public async Task<Parent?> GetAsync(Expression<Func<Parent, bool>> predicate)
        {
            return await _context.Parents
                .Include(x => x.Profile)
                .SingleOrDefaultAsync(predicate);
        }
    }
}
