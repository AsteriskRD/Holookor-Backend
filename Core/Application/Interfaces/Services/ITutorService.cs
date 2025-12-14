using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Interfaces.Services
{
    public interface ITutorService
    {
        Task<BaseResponse<TutorDto>> Register(CreateTutorRequest model, string userId);
        Task<BaseResponse<TutorDto>> GetById(string id);
        Task<BaseResponse<ICollection<TutorDto>>> GetAll(Paging paging);
    }
}
