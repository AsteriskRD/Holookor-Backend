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
            var profileId = User.FindFirstValue("userProfileId")!;
            if (string.IsNullOrEmpty(profileId))
                return Unauthorized(new { status = false, message = "UserProfileId claim missing" });

            var profile = await _profileRepo.Get(profileId);
            if (string.IsNullOrEmpty(profileId))
                return Unauthorized(new { status = false, message = "UserProfileId claim missing" });

            await _service.SendCodeAsync(profile!);
            return Ok(new { message = "Verification code sent" });
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
