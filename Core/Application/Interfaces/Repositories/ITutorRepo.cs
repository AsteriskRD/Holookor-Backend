using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface ITutorRepo : IBaseRepositoriesResponse<Tutor>
    {
        Task<Tutor> Get(string id);
        Task<Tutor> GetAsync(Expression<Func<Tutor, bool>> predicate);
        Task<ICollection<Tutor>> GetAll(Paging paging);

    }
}
