using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HolookorBackend.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/email-verification")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IUserProfileRepo _profileRepo;
        private readonly IEmailVerificationService _service;

        public EmailVerificationController(
            IEmailVerificationService service,
            IUserProfileRepo profileRepo)
        {
            _service = service;
            _profileRepo = profileRepo;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send()
        {
            var profileId = User.FindFirstValue("userProfileId");
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { status = false, message = "email claim missing" });

            var firstName = User.FindFirstValue(ClaimTypes.) ?? "User";

            if (string.IsNullOrEmpty(profileId))
                return Unauthorized(new { message = "Profile ID missing from token" });

            await _service.SendCodeAsync(profileId, email!, firstName);

            return Ok(new
            {
                message = $"Verification code sent to {email}"
            });
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(string code)
        {
            var profileId = User.FindFirstValue("userProfileId")!;
            var result = await _service.ConfirmCode(profileId, code);

            return result
                ? Ok(new { message = "Email verified successfully" })
                : BadRequest(new { message = "Invalid or expired code" });
        }
    }

}
