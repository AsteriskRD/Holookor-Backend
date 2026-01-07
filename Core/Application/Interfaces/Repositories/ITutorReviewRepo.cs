using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface ITutorReviewRepo : IBaseRepositoriesResponse<TutorReview>
    {
        Task<ICollection<TutorReview>> GetByTutorIdAsync(string tutorId);
    }
}
