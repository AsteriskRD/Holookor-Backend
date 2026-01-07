using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HolookorBackend.Presentation.Controllers
{
    [ApiController]
    [Route("api/tutors/{tutorId}/reviews")]
    public class TutorReviewController : ControllerBase
    {
        private readonly ITutorReviewService _service;

        public TutorReviewController(ITutorReviewService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(string tutorId, [FromBody] CreateTutorReviewRequest request)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await _service.AddReviewAsync(tutorId, studentId, request);
            return Ok(result);

        }


        [HttpGet]
        public async Task<IActionResult> GetReviews(string tutorId)
        {
            var result = await _service.GetTutorReviewsAsync(tutorId);
            return Ok(result);
        }
    }

}
