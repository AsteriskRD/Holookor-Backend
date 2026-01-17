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
            try
            {
                return await _context.Parents
                    .Include(x => x.Profile)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving Parent by id: {id}.", ex);
            }
        }

        public async Task<ICollection<Parent>> GetAll(Paging paging)
        {
            try
            {
                return await _context.Parents
                    .Include(x => x.Profile)
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Parents with paging.", ex);
            }
        }

        public async Task<Parent?> GetAsync(Expression<Func<Parent, bool>> predicate)
        {
            try
            {
                return await _context.Parents
                    .Include(x => x.Profile)
                    .SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Parent by predicate.", ex);
            }
        }
    }
}
