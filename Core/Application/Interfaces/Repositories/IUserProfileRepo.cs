using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface IUserProfileRepo : IBaseRepositoriesResponse<UserProfile>
    {
        Task<UserProfile> Get(string id);
        Task<UserProfile> GetAsync(Expression<Func<UserProfile, bool>> predicate);
        Task<ICollection<UserProfile>> GetAll(Paging paging);
    }
}
