using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Interfaces.Repositories;

public interface ISessionRepo : IBaseRepositoriesResponse<Session>
{
    Task<Session> GetByIdAsync(string id);
    Task<Session> GetAsync(Expression<Func<Session, bool>> predicate);
    Task<ICollection<Session>> GetStudentSessionsAsync(string studentId, Paging paging);
    Task<ICollection<Session>> GetTutorSessionsAsync(string tutorId, Paging paging);
}