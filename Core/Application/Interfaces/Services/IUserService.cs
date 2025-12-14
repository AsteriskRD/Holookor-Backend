using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse<UserDto>> Register(RegisterRequestModel model);

        Task<BaseResponse<LoginResponseModel>> Login(LoginRequestModel model);

        Task<BaseResponse<UserDto>> GetById(string id);

        Task<BaseResponse<ICollection<UserDto>>> GetAll(Paging paging);

        Task<BaseResponse<UserDto>> Update(string id, UpdateUserRequestModel model);
        Task<BaseResponse<UserDto>> ForgetPassword(string id, ForgetPasswordRequestModel model);
    }
}
