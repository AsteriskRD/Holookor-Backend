using HolookorBackend.Core.Application.DTOs.HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Interfaces.Services
{
    public interface IParentService
    {
        Task<BaseResponse<ParentDto>> Register(CreateParentRequest model);
        Task<BaseResponse<ParentDto>> Update(string id, UpdateParentRequest model);
        Task<BaseResponse<ParentDto>> GetById(string id);
        Task<BaseResponse<ICollection<ParentDto>>> GetAll(Paging paging);
    }
}
