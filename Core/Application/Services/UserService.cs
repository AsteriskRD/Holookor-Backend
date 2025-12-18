using HolookorBackend.Core.Application.Authentication;
using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.DTOs.HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Application.Mappers;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IUserProfileRepo _userProfileRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly ITutorRepo _tutorRepo;
        private readonly IParentRepo _parentRepo;
        private readonly IJWTAuthenticationManager _jwtAuthManager;

        public UserService(
            IUserRepo userRepo,
            IUserProfileRepo userProfileRepo,
            IStudentRepo studentRepo,
            ITutorRepo tutorRepo,
            IParentRepo parentRepo,
            IJWTAuthenticationManager jwtAuthManager)
        {
            _userRepo = userRepo;
            _userProfileRepo = userProfileRepo;
            _studentRepo = studentRepo;
            _tutorRepo = tutorRepo;
            _parentRepo = parentRepo;
            _jwtAuthManager = jwtAuthManager;
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

            var user = new User
            {
                Email = model.Email,
                PassWord = hashedPassword,
                UserProfileId = userProfile.Id
            };

            await _userProfileRepo.CreateAsync(userProfile);
            await _userRepo.CreateAsync(user);
            await _userRepo.SaveAsync();

            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Registration successful",
                Data = new UserDto(user.Id)
                {
                    Email = user.Email,
                    FirstName = userProfile.FirstName,
                    UserProfileId = userProfile.Id
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
                Email = user.Email,
               FirstName = user.UserProfile.FirstName,
               UserProfileId = user.UserProfileId

            });

            return new BaseResponse<LoginResponseModel>
            {
                Status = true,
                Message = "Login successful",
                Data = new LoginResponseModel(
                    user.Id,
                    user.Email,
                    user.UserProfile.FirstName,
                    user.UserProfileId
                )
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
                    UserProfileId = user.UserProfileId
                }
            };
        }

        public async Task<BaseResponse<ICollection<UserDto>>> GetAll(Paging paging)
        {
            var users = await _userRepo.GetAll(paging);

            var result = users.Select(u => new UserDto(u.Id)
            {
                Email = u.Email,
                FirstName = u.UserProfile.FirstName,
                UserProfileId = u.UserProfileId
            }).ToList();

            return new BaseResponse<ICollection<UserDto>>
            {
                Status = true,
                Data = result
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

            if (string.IsNullOrWhiteSpace(model.CurrentPassword) ||
                string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return new BaseResponse<UserDto>
                {
                    Status = false,
                    Message = "Current and new passwords are required"
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
                    Message = "New password cannot be same as old password"
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
    public async Task<BaseResponse<ProfileDto>> GetByProfile(string userProfileId)
    {
        var profile = await _userProfileRepo.Get(userProfileId);
        if (profile == null)
        {
            return new BaseResponse<ProfileDto>
            {
                Status = false,
                Message = "Profile not found"
            };
        }

        StudentDto? studentDto = null;
        TutorDto? tutorDto = null;
        ParentDto? parentDto = null;

        var student = await _studentRepo.GetAsync(s => s.UserProfileId == profile.Id);
        if (student != null)
        {
            studentDto = StudentMapper.Map(student);
        }

        var tutor = await _tutorRepo.GetAsync(t => t.UserProfileId == profile.Id);
        if (tutor != null)
        {
            tutorDto = TutorMapper.Map(tutor);
        }

        var parent = await _parentRepo.GetAsync(p => p.UserProfileId == profile.Id);
        if (parent != null)
        {
            var children = await _studentRepo.GetAllAsync(
                s => s.ParentId == parent.Id
            );

            parentDto = ParentMapper.Map(parent, children);
        }

        var dto = new ProfileDto(
            profile.Id,
            profile.FirstName,
            profile.LastName,
            profile.PhoneNumber,
            profile.Users!.Email,
            profile.Role,
            studentDto,
            tutorDto,
            parentDto
        );

        return new BaseResponse<ProfileDto>
        {
            Status = true,
            Data = dto
        };
    }


    public Task<BaseResponse<UserDto>> ForgetPassword(string id, ForgetPasswordRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
