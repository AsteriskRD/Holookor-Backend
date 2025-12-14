using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HolookorBackend.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentRequest model)
        {
            var userProfileId = User.FindFirstValue("userProfileId")!;
            return Ok(await _service.Create(model, userProfileId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
            => Ok(await _service.GetById(id));

        [HttpGet("child/{childId}")]
        public async Task<IActionResult> GetByChildId(string childId)
            => Ok(await _service.GetByChildID(childId));
    }

}
