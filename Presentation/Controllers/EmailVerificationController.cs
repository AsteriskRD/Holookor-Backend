using HolookorBackend.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HolookorBackend.Presentation.Controllers
{
    [ApiController]
    [Route("api/email-verification")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IEmailVerificationService _service;

        public EmailVerificationController(
            IEmailVerificationService service)
        {
            _service = service;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromQuery] string userProfileId,[FromQuery] string email, [FromQuery]string firstName)
        {
            //var profileId = User.FindFirstValue("userProfileId");
            //var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { message = "Email missing" });

            //var firstName = User.FindFirstValue(ClaimTypes.GivenName) ?? "User";

            if (string.IsNullOrEmpty(userProfileId))
                return Unauthorized(new { message = "Profile ID missing" });

            await _service.SendCodeAsync(userProfileId, email!, firstName);

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
