using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface IUserRepo : IBaseRepositoriesResponse<User>
    {
        Task<User> Get(string id);
        Task<User> GetAsync(Expression<Func<User, bool>> predicate);
        Task<ICollection<User>> GetAll(Paging paging);
    }
}
