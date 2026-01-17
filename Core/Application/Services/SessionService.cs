using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Exceptions.HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Persistence;

public sealed class SessionService : ISessionService
{
    private readonly ISessionRepo _sessionRepo;
    private readonly ITutorRepo _tutorRepo;
    private readonly IStudentRepo _studentRepo;
    private readonly IPriceingConfigRepo _pricingRepo;

    public SessionService(ISessionRepo sessionRepo, ITutorRepo tutorRepo, IStudentRepo studentRepo, IPriceingConfigRepo pricingRepo)
    {
        _sessionRepo = sessionRepo;
        _tutorRepo = tutorRepo;
        _studentRepo = studentRepo;
        _pricingRepo = pricingRepo;
    }

    public async Task<BookingSummaryDto> BookSessionAsync(string studentId, BookSessionRequest request)
    {
        try
        {
            var tutor = await _tutorRepo.Get(request.TutorId)
                ?? throw new DomainException("Tutor not found");

            var student = await _studentRepo.Get(studentId)
                ?? throw new DomainException("Student not found");

            var pricing = await _pricingRepo.GetActiveAsync()
                ?? throw new DomainException("Pricing configuration missing");

            if (!tutor.Subjects.Contains(request.Subject))
                throw new DomainException("Tutor does not teach this subject");

            var availability = tutor.Availabilities.FirstOrDefault(a =>
                a.DayOfWeek == request.DayOfWeek &&
                request.TutorStartTime >= a.StartTime &&
                request.TutorStartTime.Add(TimeSpan.FromHours(request.DurationHours)) <= a.EndTime
            );

            if (availability is null)
                throw new DomainException("Selected time is outside tutor availability");

            var tutorLocalStart = BuildTutorLocalDate(request.DayOfWeek, request.TutorStartTime);

            var startUtc = ConvertTutorLocalToUtc(tutorLocalStart, tutor.TimeZoneId);

            var endUtc = startUtc.AddHours(request.DurationHours);

            var basePrice = tutor.HourlyRate * request.DurationHours;
            var tax = basePrice * pricing.TaxPercentage / 100;

            var session = new Session(
                tutor.Id,
                student.Id,
                startUtc,
                endUtc,
                request.Subject,
                tutor.HourlyRate,
                pricing.ServiceFee,
                tax,
                request.Topic,
                request.Note
            );

            await _sessionRepo.CreateAsync(session);

            return new BookingSummaryDto
            {
                SessionId = session.Id,
                TutorName = tutor.Profile.FirstName,
                Subject = session.Subject,
                Topic = session.Topic,
                SessionCount = 1,
                StartTimeUtc = startUtc,
                EndTimeUtc = endUtc,
                BasePrice = basePrice,
                ServiceFee = pricing.ServiceFee,
                Tax = tax,
                TotalAmount = session.TotalAmount
            };
        }
        catch (DomainException)
        {
            throw; // Let DomainExceptions bubble up
        }
        catch (Exception ex)
        {
            // Only take ex.Message, as DomainException only has one-arg constructor.
            throw new DomainException($"Failed to book session: {ex.Message}");
        }
    }

    public async Task<IReadOnlyList<StudentSessionDto>> GetStudentSessionsAsync(string studentId, Paging paging)
    {
        try
        {
            var student = await _studentRepo.Get(studentId)
                ?? throw new DomainException("Student not found");

            var tz = TimeZoneInfo.FindSystemTimeZoneById(student.TimeZoneId);

            var sessions = await _sessionRepo.GetStudentSessionsAsync(studentId, paging);

            return sessions.Select(s => new StudentSessionDto
            {
                SessionId = s.Id,
                TutorName = s.Tutor.Profile.FirstName,
                StartTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(s.StartTimeUtc, tz),
                EndTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(s.EndTimeUtc, tz),
                CanJoinClass = s.CanJoin(DateTime.UtcNow)
            }).ToList();
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DomainException($"Failed to retrieve student sessions: {ex.Message}");
        }
    }

    public async Task<IReadOnlyList<TutorSessionDto>> GetTutorSessionsAsync(string tutorId, Paging paging)
    {
        try
        {
            var tutor = await _tutorRepo.Get(tutorId)
                ?? throw new DomainException("Tutor not found");

            var tz = TimeZoneInfo.FindSystemTimeZoneById(tutor.TimeZoneId);

            var sessions = await _sessionRepo.GetTutorSessionsAsync(tutorId, paging);

            return sessions.Select(s => new TutorSessionDto
            {
                SessionId = s.Id,
                StudentId = s.Student.Id,
                StudentName = s.Student.Profile.FirstName,
                StartTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(s.StartTimeUtc, tz),
                EndTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(s.EndTimeUtc, tz),
                CanJoinClass = s.CanJoin(DateTime.UtcNow)
            }).ToList();
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DomainException($"Failed to retrieve tutor sessions: {ex.Message}");
        }
    }

    private static DateTime BuildTutorLocalDate(DayOfWeek day, TimeSpan time)
    {
        var today = DateTime.Today;
        var daysUntil = ((int)day - (int)today.DayOfWeek + 7) % 7;
        return today.AddDays(daysUntil).Add(time);
    }

    private static DateTime ConvertTutorLocalToUtc(DateTime local, string tzId)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);
        return TimeZoneInfo.ConvertTimeToUtc(local, tz);
    }
}