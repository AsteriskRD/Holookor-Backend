using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class StudentRepositories : BaseRespositories<Student>, IStudentRepo
    {
        private readonly HolookorSystem _context;
        public StudentRepositories(HolookorSystem context) : base(context)
        {
            _context = context;
        }

        public async Task<Student?> Get(string id)
        {
            return await _context.Students
                .Include(x => x.Profile)
                 .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Student>> GetAll(Paging paging)
        {
            return await _context.Students
                .Include(x => x.Profile)
                .Where(x => !x.IsDeleted)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();
        }

        public async Task<ICollection<Student>> GetAllAsync(
    Expression<Func<Student, bool>> predicate
)
        {
            return await _context.Students
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Student?> GetAsync(Expression<Func<Student, bool>> predicate)
        {
            return await _context.Students
                .Include(x => x.Profile)
                .SingleOrDefaultAsync(predicate);
        }
    }
}
