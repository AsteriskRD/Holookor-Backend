using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Responses
{
    public interface IBaseRepositoriesResponse<T>
    {
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        bool Check(Expression<Func<T, bool>> predicate);
        Task<int> SaveAsync();
    }
}
