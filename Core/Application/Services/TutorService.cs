using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Exceptions.HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Core.Domain.Enums;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
            try
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
                    tutor.AddAvailability(a.DayOfWeek, a.StartTime, a.EndTime);

                tutor.AssignProfile(profile.Id);

                await _tutorRepo.CreateAsync(tutor);
                await _tutorRepo.SaveAsync();

                return Success(Map(tutor));
            }
            catch (ValidationException ve)
            {
                return new BaseResponse<TutorDto>
                {
                    Status = false,
                    Message = ve.Message
                };
            }
            catch (NotFoundException nfe)
            {
                return new BaseResponse<TutorDto>
                {
                    Status = false,
                    Message = nfe.Message
                };
            }
            catch (DomainException de)
            {
                return new BaseResponse<TutorDto>
                {
                    Status = false,
                    Message = de.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<TutorDto>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }


        public async Task<BaseResponse<TutorDto>> GetById(string id)
        {
            try
            {
                var tutor = await _tutorRepo.Get(id)
                    ?? throw new NotFoundException("Tutor not found");

                return Success(Map(tutor));
            }
            catch (NotFoundException nfe)
            {
                return new BaseResponse<TutorDto>
                {
                    Status = false,
                    Message = nfe.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<TutorDto>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ICollection<TutorDto>>> GetAll(Paging paging)
        {
            try
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
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<TutorDto>>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ICollection<TutorSearchResponseDto>>> SearchAsync(TutorSearchRequestDto filter, Paging paging)
        {
            try
            {
                var predicate = PredicateBuilder.True<Tutor>();

                predicate = predicate.And(t => t.IsVerified && t.IsAvailableStatus);

                if (!string.IsNullOrWhiteSpace(filter.Subject))
                    predicate = predicate.And(t => t.Subjects.Contains(filter.Subject));

                if (filter.Gender.HasValue)
                    predicate = predicate.And(t => t.Gender == filter.Gender);

                if (filter.MinRate.HasValue)
                    predicate = predicate.And(t => t.HourlyRate >= filter.MinRate);

                if (filter.MaxRate.HasValue)
                    predicate = predicate.And(t => t.HourlyRate <= filter.MaxRate);

                if (filter.MinRating.HasValue)
                    predicate = predicate.And(t =>
                        t.Reviews.Any() &&
                        t.Reviews.Average(r => r.Rating) >= filter.MinRating);

                var query = _tutorRepo.Query(predicate);

                query = filter.SortBy switch
                {
                    TutorSortOption.PriceLowToHigh =>
                        query.OrderBy(t => t.HourlyRate),

                    TutorSortOption.PriceHighToLow =>
                        query.OrderByDescending(t => t.HourlyRate),

                    TutorSortOption.RatingHighToLow =>
                        query.OrderByDescending(t =>
                            t.Reviews.Any() ? t.Reviews.Average(r => r.Rating) : 0),

                    TutorSortOption.ExperienceHighToLow =>
                        query.OrderByDescending(t => t.YearsOfExperience),

                    _ =>
                        query.OrderByDescending(t => t.DateCreated)
                };

                var totalCount = await query.CountAsync();

                var tutors = await query
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToListAsync();

                var result = tutors.Select(t =>
                {
                    var avgRating = t.Reviews.Any()
                        ? t.Reviews.Average(r => r.Rating)
                        : 0;

                    return new TutorSearchResponseDto(
                        t.Id,
                        $"{t.Profile.FirstName} {t.Profile.LastName}",
                        t.Gender,
                        t.HourlyRate,
                        t.IsAvailableStatus,
                        t.IsVerified,
                        Math.Round(avgRating, 1),
                        t.Reviews.Count,
                        t.Subjects.ToList()
                    );
                }).ToList();

                return new BaseResponse<ICollection<TutorSearchResponseDto>>
                {
                    Status = true,
                    Data = result,
                    TotalCount = totalCount,
                    PageNumber = paging.PageNumber,
                    PageSize = paging.PageSize
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<TutorSearchResponseDto>>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }


        private static TutorDto Map(Tutor t)
            => new(
                t.Id,
                t.Gender,
                t.DateOfBirth,
                t.Location,
                t.Qualifications.ToList(),
                t.Subjects.ToList(),
                t.Availabilities.Select(a => new AvailabilityDto(
                     a.DayOfWeek,
                     a.StartTime,
                     a.EndTime
                    )).ToList(),
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