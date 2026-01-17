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
            try
            {
                return await _context.Tutors
                     .Include(x => x.Profile)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving Tutor by id: {id}.", ex);
            }
        }

        public async Task<ICollection<Tutor>> GetAll(Paging paging)
        {
            try
            {
                return await _context.Tutors
                     .Include(x => x.Profile)
                     .Skip((paging.PageNumber - 1) * paging.PageSize)
                     .Take(paging.PageSize)
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Tutors with paging.", ex);
            }
        }

        public async Task<ICollection<Tutor>> GetAllAsync(Expression<Func<Tutor, bool>> predicate, Paging paging)
        {
            try
            {
                return await _context.Tutors
                 .Include(t => t.Profile)
                 .Include(t => t.Reviews)
                 .Where(predicate)
                 .Skip((paging.PageNumber - 1) * paging.PageSize)
                 .Take(paging.PageSize)
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Tutors by predicate with paging.", ex);
            }
        }

        public async Task<Tutor?> GetAsync(Expression<Func<Tutor, bool>> predicate)
        {
            try
            {
                return await _context.Tutors
                    .Include(x => x.Profile)
                    .SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Tutor by predicate.", ex);
            }
        }

        public IQueryable<Tutor> Query(Expression<Func<Tutor, bool>> predicate)
        {
            return _context.Tutors
                .Include(t => t.Profile)
                .Include(t => t.Reviews)
                .Where(predicate);
        }

    }
}
