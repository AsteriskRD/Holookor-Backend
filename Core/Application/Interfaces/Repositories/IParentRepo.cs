using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface IParentRepo : IBaseRepositoriesResponse<Parent>
    {
        Task<Parent> Get(string id);
        Task<Parent> GetAsync(Expression<Func<Parent, bool>> predicate);
        Task<ICollection<Parent>> GetAll(Paging paging);
    }
}
