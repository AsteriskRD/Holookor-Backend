using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface IStudentRepo : IBaseRepositoriesResponse<Student>
    {
        Task<Student> Get(string id);
        Task<Student> GetAsync(Expression<Func<Student, bool>> predicate);
        Task<ICollection<Student>> GetAll(Paging paging);
    }
}
