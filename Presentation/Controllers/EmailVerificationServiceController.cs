using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Services;
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
        private readonly EmailVerificationService _service;
        private readonly IUserProfileRepo _profileRepo;

        public EmailVerificationController(
            EmailVerificationService service,
            IUserProfileRepo profileRepo)
        {
            _service = service;
            _profileRepo = profileRepo;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send()
        {
            var profileId = User.FindFirstValue("userProfileId")!;
            var profile = await _profileRepo.Get(profileId);

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
