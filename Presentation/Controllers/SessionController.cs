using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/sessions")]
public sealed class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("book")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> BookSession([FromBody] BookSessionRequest request)
    {
        var studentId = User.FindFirst("id")!.Value;

        var summary = await _sessionService
            .BookSessionAsync(studentId, request);

        return Ok(summary);
    }

    [HttpGet("student")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetStudentSessions([FromQuery] Paging paging)
    {
        var studentId = User.FindFirst("id")!.Value;

        var sessions = await _sessionService
            .GetStudentSessionsAsync(studentId, paging);

        return Ok(sessions);
    }

    [HttpGet("tutor")]
    [Authorize(Roles = "Tutor")]
    public async Task<IActionResult> GetTutorSessions([FromQuery] Paging paging)
    {
        var tutorId = User.FindFirst("id")!.Value;

        var sessions = await _sessionService
            .GetTutorSessionsAsync(tutorId, paging);

        return Ok(sessions);
    }
}
