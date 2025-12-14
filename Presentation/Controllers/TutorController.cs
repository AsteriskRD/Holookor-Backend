using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HolookorBackend.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tutors")]
    public class TutorsController : ControllerBase
    {
        private readonly ITutorService _service;

        public TutorsController(ITutorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateTutorRequest model)
        {
            var userProfileId = User.FindFirstValue("userProfileId")!;
            return Ok(await _service.Register(model, userProfileId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
            => Ok(await _service.GetById(id));
    }

}
