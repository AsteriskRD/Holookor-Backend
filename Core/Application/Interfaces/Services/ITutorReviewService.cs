using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Responses;

namespace HolookorBackend.Core.Application.Interfaces.Services
{
    public interface ITutorReviewService
    {
        Task<BaseResponse<TutorReviewDto>> AddReviewAsync(string tutorId, string studentId,CreateTutorReviewRequest request);

        Task<BaseResponse<ICollection<TutorReviewDto>>> GetTutorReviewsAsync(string tutorId);
    }

}
