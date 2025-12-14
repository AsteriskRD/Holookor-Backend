using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Core.Domain.Enums;

namespace HolookorBackend.Core.Application.DTOs
{
    public record StudentDto(
        string Id,
        string SchoolLevel,
        DateOnly DateOfBirth,
        Gender Gender,
        WeeklyAvailability WeeklyAvailability,
        Location Location,
        string TimeZoneId,
        PreferredClassTime PreferredClassTime,
        IReadOnlyCollection<string> SubjectOfInterest,
        string UserProfileId,
        string ChildID
    );

    public record CreateStudentRequest(
        string SchoolLevel,
        DateOnly DateOfBirth,
        Gender Gender,
        WeeklyAvailability WeeklyAvailability,
        Location Location,
        string TimeZoneId,
        PreferredClassTime PreferredClassTime,
        string[] SubjectOfInterest
    );

    public record UpdateStudentRequest(
        string? SchoolLevel = null,
        DateOnly? DateOfBirth = null,
        Gender? Gender = null,
        WeeklyAvailability? WeeklyAvailability = null,
        Location? Location = null,
        string? TimeZoneId = null,
        PreferredClassTime? PreferredClassTime = null,
        string[]? SubjectOfInterest = null
    );
}

