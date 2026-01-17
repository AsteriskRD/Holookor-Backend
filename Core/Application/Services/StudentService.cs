using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Exceptions.HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Services
{
    public sealed class StudentService : IStudentService
    {
        private readonly IStudentRepo _studentRepo;
        private readonly IUserProfileRepo _profileRepo;

        public StudentService(IStudentRepo studentRepo, IUserProfileRepo profileRepo)
        {
            _studentRepo = studentRepo;
            _profileRepo = profileRepo;
        }

        public async Task<BaseResponse<StudentDto>> Create(CreateStudentRequest model, string userId)
        {
            try
            {
                if (model.DateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
                    throw new ValidationException("Invalid date of birth");

                var profile = await _profileRepo.Get(userId)
                    ?? throw new NotFoundException("User profile not found");

                if (!profile.IsEmailVerified)
                    throw new DomainException("Email must be verified before creating a student");

                var student = new Student(
                    model.SchoolLevel,
                    model.DateOfBirth,
                    model.Gender,
                    model.WeeklyAvailability,
                    model.Location,
                    model.TimeZoneId,
                    model.PreferredClassTime,
                    model.SubjectOfInterest
                );

                student.AssignProfile(profile.Id);
                if (profile.Role == "Parent")
                {
                    student.AssignParent(profile.Id);
                }

                await _studentRepo.CreateAsync(student);
                await _studentRepo.SaveAsync();

                return Success(Map(student));
            }
            catch (ValidationException ve)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = ve.Message
                };
            }
            catch (NotFoundException nfe)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = nfe.Message
                };
            }
            catch (DomainException de)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = de.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<StudentDto>> Update(string id, UpdateStudentRequest model)
        {
            try
            {
                var student = await _studentRepo.Get(id)
                    ?? throw new NotFoundException("Student not found");

                if (model.Location != null)
                    student.UpdateLocation(model.Location);

                if (model.PreferredClassTime.HasValue)
                    student.UpdatePreferredClassTime(model.PreferredClassTime.Value);

                _studentRepo.Update(student);
                await _studentRepo.SaveAsync();

                return Success(Map(student));
            }
            catch (NotFoundException nfe)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = nfe.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<StudentDto>> GetById(string id)
        {
            try
            {
                var student = await _studentRepo.Get(id)
                    ?? throw new NotFoundException("Student not found");

                return Success(Map(student));
            }
            catch (NotFoundException nfe)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = nfe.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ICollection<StudentDto>>> GetAll(Paging paging)
        {
            try
            {
                var students = await _studentRepo.GetAll(paging);
                return new BaseResponse<ICollection<StudentDto>>
                {
                    Data = students.Select(Map).ToList(),
                    TotalCount = students.Count,
                    Status = true,
                    PageNumber = paging?.PageNumber ?? 1,
                    PageSize = paging?.PageSize ?? students.Count
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<StudentDto>>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<StudentDto>> GetByChildID(string childId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(childId))
                    throw new ValidationException("ChildID is required");

                var student = await _studentRepo.GetAsync(s => s.ChildID == childId && !s.IsDeleted)
                    ?? throw new NotFoundException("Student not found");

                return Success(Map(student));
            }
            catch (ValidationException ve)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = ve.Message
                };
            }
            catch (NotFoundException nfe)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = nfe.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudentDto>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        private static StudentDto Map(Student s)
            => new(
                s.Id,
                s.SchoolLevel,
                s.DateOfBirth,
                s.Gender,
                s.WeeklyAvailability,
                s.Location,
                s.TimeZoneId,
                s.PreferredClassTime,
                s.SubjectOfInterest.ToList(),
                s.UserProfileId,
                s.ChildID
            );

        private static BaseResponse<T> Success<T>(T data, int total = 0, Paging? paging = null)
            => new()
            {
                Status = true,
                Data = data,
                TotalCount = total,
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? total
            };
    }
} 
