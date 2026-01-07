using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class TutorReviewRepositories : BaseRespositories<TutorReview>, ITutorReviewRepo
    {
        private readonly HolookorSystem _context;

        public TutorReviewRepositories(HolookorSystem context) : base(context) 
        {
            _context = context;
        }
        public async Task<ICollection<TutorReview>> GetByTutorIdAsync(string tutorId)
        {
            return await _context.TutorReviews
                .Where(r => r.TutorId == tutorId)
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();
        }
    }
}
