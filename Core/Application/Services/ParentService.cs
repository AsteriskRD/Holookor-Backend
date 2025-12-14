using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.DTOs.HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Exceptions.HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;

namespace HolookorBackend.Core.Application.Services
{
    public sealed class ParentService : IParentService
    {
        private readonly IParentRepo _parentRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly IUserProfileRepo _userProfileRepo;

        public ParentService(IParentRepo parentRepo, IStudentRepo studentRepo, IUserProfileRepo userProfileRepo)
        {
            _parentRepo = parentRepo;
            _studentRepo = studentRepo;
            _userProfileRepo = userProfileRepo;
        }

        public async Task<BaseResponse<ParentDto>> Register(CreateParentRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.UserProfileId))
                throw new ValidationException("UserProfileId is required");

            var profile = await _userProfileRepo.Get(model.UserProfileId)
                ?? throw new NotFoundException("User profile not found");

            if (!profile.IsEmailVerified)
                throw new DomainException("Email must be verified before creating a parent");

            var parent = new Parent
            {
                UserProfileId = profile.Id
            };

            if (model.ChildrenIds != null && model.ChildrenIds.Any())
            {
                foreach (var childId in model.ChildrenIds)
                {
                    var student = await _studentRepo.Get(childId)
                        ?? throw new NotFoundException($"Student with id {childId} not found");

                    parent.Children.Add(student);
                }
            }

            await _parentRepo.CreateAsync(parent);
            await _parentRepo.SaveAsync();

            return Success(Map(parent));
        }


        public async Task<BaseResponse<ParentDto>> Update(string id, UpdateParentRequest model)
        {
            var parent = await _parentRepo.Get(id)
                ?? throw new NotFoundException("Parent not found");

            parent.Children.Clear();

            if (model.ChildrenIds != null)
            {
                foreach (var childId in model.ChildrenIds)
                {
                    var student = await _studentRepo.Get(childId)
                        ?? throw new NotFoundException($"Student with id {childId} not found");

                    parent.Children.Add(student);
                }
            }

            _parentRepo.Update(parent);
            await _parentRepo.SaveAsync();

            return Success(Map(parent));
        }

        public async Task<BaseResponse<ParentDto>> GetById(string id)
        {
            var parent = await _parentRepo.Get(id)
                ?? throw new NotFoundException("Parent not found");

            return Success(Map(parent));
        }

        public async Task<BaseResponse<ICollection<ParentDto>>> GetAll(Paging paging)
        {
            var parents = await _parentRepo.GetAll(paging);

            return new BaseResponse<ICollection<ParentDto>>
            {
                Data = parents.Select(Map).ToList(),
                TotalCount = parents.Count,
                Status = true,
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? parents.Count
            };
        }

        private static ParentDto Map(Parent parent)
        {
            return new ParentDto(
                parent.Id,
                parent.UserProfileId,
                parent.Children.Select(s => new StudentDto(
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
                )).ToList()
            );
        }

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
