using HolookorBackend.Core.Application.DTOs.HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HolookorBackend.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/parents")]
    public class ParentsController : ControllerBase
    {
        private readonly IParentService _service;

        public ParentsController(IParentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateParentRequest model)
            => Ok(await _service.Register(model));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateParentRequest model)
            => Ok(await _service.Update(id, model));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
            => Ok(await _service.GetById(id));
    }

}
