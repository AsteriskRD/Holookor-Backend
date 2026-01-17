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
            try
            {
                return await _context.Students
                    .Include(x => x.Profile)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving Student by id: {id}.", ex);
            }
        }

        public async Task<ICollection<Student>> GetAll(Paging paging)
        {
            try
            {
                return await _context.Students
                    .Include(x => x.Profile)
                    .Where(x => !x.IsDeleted)
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Students with paging.", ex);
            }
        }

        public async Task<ICollection<Student>> GetAllAsync(Expression<Func<Student, bool>> predicate)
        {
            try
            {
                return await _context.Students
                    .Where(predicate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Students by predicate.", ex);
            }
        }

        public async Task<Student?> GetAsync(Expression<Func<Student, bool>> predicate)
        {
            try
            {
                return await _context.Students
                    .Include(x => x.Profile)
                    .SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving Student by predicate.", ex);
            }
        }
    }
}
