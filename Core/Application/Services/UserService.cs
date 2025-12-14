using HolookorBackend.Core.Application.Authentication;
using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IUserProfileRepo _userProfileRepo;
        private readonly IJWTAuthenticationManager _jwtAuthManager;

        public UserService(IUserRepo userRepo, IJWTAuthenticationManager jwtAuthManager, IUserProfileRepo userProfileRepo)
        {
            _userRepo = userRepo;
            _jwtAuthManager = jwtAuthManager;
            _userProfileRepo = userProfileRepo;
        }

        public async Task<BaseResponse<UserDto>> Register(RegisterRequestModel model)
        {
            var existingUser = await _userRepo.GetAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "Email already registered"
                };
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var userProfile = new UserProfile(
                model.FirstName,
                model.LastName,
                model.Role,
                model.PhoneNumber
              );



            var newUser = new User
            {
                Email = model.Email,
                PassWord = hashedPassword,
                UserProfileId = userProfile.Id,
            };

            await _userProfileRepo.CreateAsync(userProfile);
            await _userRepo.CreateAsync(newUser);
            await _userRepo.SaveAsync();

            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Registration successful",
                Data = new UserDto(newUser.Id)
                {
                    Email = newUser.Email,
                    FirstName = userProfile.FirstName
                }
            };
        }

        public async Task<BaseResponse<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await _userRepo.GetAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PassWord))
            {
                return new BaseResponse<LoginResponseModel>
                {
                    Status = false,
                    Message = "Invalid email or password"
                };
            }

            var token = _jwtAuthManager.GenerateToken(new UserDto(user.Id)
            {
                Email = user.Email
            });
            return new BaseResponse<LoginResponseModel>
            {
                Status = true,
                Message = "Login successful",
                Data = new LoginResponseModel(user.Id, user.Email, user.UserProfile.FirstName,user.UserProfileId)
                {
                    Token = token
                }
            };
        }

        public async Task<BaseResponse<UserDto>> GetById(string id)
        {
            var user = await _userRepo.Get(id);
            if (user == null)
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            return new BaseResponse<UserDto>
            {
                Status = true,
                Data = new UserDto(user.Id)
                {
                    Email = user.Email,
                    FirstName = user.UserProfile.FirstName,
                }
            };
        }

        public async Task<BaseResponse<ICollection<UserDto>>> GetAll(Paging paging)
        {
            var users = await _userRepo.GetAll(paging);
            var dtos = users.Select(u => new UserDto(u.Id)
            {
                Email = u.Email
            }).ToList();

            return new BaseResponse<ICollection<UserDto>>
            {
                Status = true,
                Data = dtos
            };
        }

        public async Task<BaseResponse<UserDto>> Update(string id, UpdateUserRequestModel model)
        {
            var user = await _userRepo.Get(id);
            if (user == null)
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            if (string.IsNullOrWhiteSpace(model.CurrentPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "Both current and new passwords must be provided"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PassWord))
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "Current password is incorrect"
                };
            }

            if (BCrypt.Net.BCrypt.Verify(model.NewPassword, user.PassWord))
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "New password cannot be the same as current password"
                };
            }

            user.PassWord = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            _userRepo.Update(user);
            await _userRepo.SaveAsync();

            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Password updated successfully",
                Data = new UserDto(user.Id)
                {
                    Email = user.Email
                }
            };
        }

        public Task<BaseResponse<UserDto>> ForgetPassword(string id, ForgetPasswordRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
