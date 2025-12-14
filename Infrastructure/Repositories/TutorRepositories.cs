using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class TutorRepositories : BaseRespositories<Tutor>, ITutorRepo
    {
        private readonly HolookorSystem _context;
        public TutorRepositories(HolookorSystem context) : base(context)
        {
            _context = context;
        }

        public async Task<Tutor?> Get(string id)
        {
            return await _context.Tutors
                 .Include(x => x.Profile)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Tutor>> GetAll(Paging paging)
        {
            return await _context.Tutors
                 .Include(x => x.Profile)
                 .Skip((paging.PageNumber - 1) * paging.PageSize)
                 .Take(paging.PageSize)
                 .ToListAsync();
        }

        public async Task<Tutor?> GetAsync(Expression<Func<Tutor, bool>> predicate)
        {
            return await _context.Tutors
                .Include(x => x.Profile)
                .SingleOrDefaultAsync(predicate);
        }
    }
}
