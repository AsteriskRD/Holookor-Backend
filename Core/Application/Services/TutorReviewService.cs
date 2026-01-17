using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Exceptions.HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Services
{
    public class TutorReviewService : ITutorReviewService
    {
        private readonly ITutorReviewRepo _reviewRepo;
        private readonly ITutorRepo _tutorRepo;

        public TutorReviewService(
            ITutorReviewRepo reviewRepo,
            ITutorRepo tutorRepo)
        {
            _reviewRepo = reviewRepo;
            _tutorRepo = tutorRepo;
        }

        public async Task<BaseResponse<TutorReviewDto>> AddReviewAsync(string tutorId, string studentId, CreateTutorReviewRequest request)
        {
            try
            {
                var tutor = await _tutorRepo.Get(tutorId)
                    ?? throw new NotFoundException("Tutor not found");

                var review = new TutorReview(
                    tutorId,
                    studentId,
                    request.Rating,
                    request.Comment
                );

                await _reviewRepo.CreateAsync(review);
                await _reviewRepo.SaveAsync();

                return new BaseResponse<TutorReviewDto>
                {
                    Status = true,
                    Message = "Review added successfully"
                };
            }
            catch (NotFoundException nfe)
            {
                return new BaseResponse<TutorReviewDto>
                {
                    Status = false,
                    Message = nfe.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<TutorReviewDto>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ICollection<TutorReviewDto>>> GetTutorReviewsAsync(string tutorId)
        {
            try
            {
                var reviews = await _reviewRepo.GetByTutorIdAsync(tutorId);

                var data = reviews.Select(r =>
                    new TutorReviewDto(
                        r.Id,
                        r.StudentId,
                        r.Rating,
                        r.Comment,
                        r.CreatedOn
                    )
                ).ToList();

                return new BaseResponse<ICollection<TutorReviewDto>>
                {
                    Status = true,
                    Data = data,
                    TotalCount = data.Count
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<TutorReviewDto>>
                {
                    Status = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }
    }

}
