using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Interfaces.Services
{
    public interface ISessionService
    {
        Task<BookingSummaryDto> BookSessionAsync(string studentId, BookSessionRequest request);

        Task<IReadOnlyList<StudentSessionDto>> GetStudentSessionsAsync(string studentId, Paging paging);
        Task<IReadOnlyList<TutorSessionDto>> GetTutorSessionsAsync(string tutorId, Paging paging);
    }
}
