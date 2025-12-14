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
    public sealed class TutorService : ITutorService
    {
        private readonly ITutorRepo _tutorRepo;
        private readonly IUserProfileRepo _userRepo;

        public TutorService(ITutorRepo tutorRepo, IUserProfileRepo userRepo)
        {
            _tutorRepo = tutorRepo;
            _userRepo = userRepo;
        }


        public async Task<BaseResponse<TutorDto>> Register(CreateTutorRequest model, string userProfileId)
        {
            if (model.HourlyRate <= 0)
                throw new ValidationException("Hourly rate must be greater than zero");

            var profile = await _userRepo.Get(userProfileId)
                ?? throw new NotFoundException("User profile not found");

            if (!profile.IsEmailVerified)
                throw new DomainException("Email must be verified before creating a tutor");

            var tutor = new Tutor(
                model.Gender,
                model.DateOfBirth,
                model.Location,
                model.YearsOfExperience,
                model.CredentialsDocument,
                model.GovernmentID,
                model.HourlyRate,
                model.Bio,
                model.TimeZoneId
            );

            foreach (var q in model.Qualifications)
                tutor.AddQualification(q);

            foreach (var s in model.Subjects)
                tutor.AddSubject(s);

            foreach (var a in model.Availability)
                tutor.AddAvailability(a);

            tutor.AssignProfile(profile.Id);

            await _tutorRepo.CreateAsync(tutor);
            await _tutorRepo.SaveAsync();

            return Success(Map(tutor));
        }


        public async Task<BaseResponse<TutorDto>> GetById(string id)
        {
            var tutor = await _tutorRepo.Get(id)
                ?? throw new NotFoundException("Tutor not found");

            return Success(Map(tutor));
        }

        public async Task<BaseResponse<ICollection<TutorDto>>> GetAll(Paging paging)
        {
            var tutors = await _tutorRepo.GetAll(paging);
            return new BaseResponse<ICollection<TutorDto>>
            {
                Data = tutors.Select(Map).ToList(),
                TotalCount = tutors.Count,
                Status = true,
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? tutors.Count
            };
        }

        private static TutorDto Map(Tutor t)
            => new(
                t.Id,
                t.Gender,
                t.DateOfBirth,
                t.Location,
                t.Qualifications.ToList(),
                t.Subjects.ToList(),
                t.Availability.ToList(),
                t.YearsOfExperience,
                t.CredentialsDocument,
                t.GovernmentID,
                t.HourlyRate,
                t.Bio,
                t.IsVerified,
                t.TimeZoneId,
                t.UserProfileId,
                t.ProfilePictureURL
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
