using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task<BaseResponse<StudentDto>> Create(CreateStudentRequest model, string id);
        Task<BaseResponse<StudentDto>> Update(string id, UpdateStudentRequest model);
        Task<BaseResponse<StudentDto>> GetById(string id);
        Task<BaseResponse<StudentDto>> GetByChildID(string childId);
        Task<BaseResponse<ICollection<StudentDto>>> GetAll(Paging paging);
    }
}
