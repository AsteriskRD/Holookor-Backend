using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HolookorBackend.Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestModel model)
            => Ok(await _service.Register(model));

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
            => Ok(await _service.Login(model));

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await _service.GetById(userId));
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword(UpdateUserRequestModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await _service.Update(userId, model));
        }

        [HttpPost("{id}/forgot-password")]
        public async Task<IActionResult> ForgetPassword(string id, [FromBody] ForgetPasswordRequestModel model)
        {
            var result = await _service.ForgetPassword(id, model);
            return Ok(result);
        }
    }
}


