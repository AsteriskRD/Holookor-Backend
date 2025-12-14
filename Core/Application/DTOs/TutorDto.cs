using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Core.Domain.Enums;

namespace HolookorBackend.Core.Application.DTOs
{
    public record TutorDto(
        string Id,
        Gender Gender,
        DateOnly DateOfBirth,
        Location Location,
        IReadOnlyCollection<string> Qualifications,
        IReadOnlyCollection<string> Subjects,
        IReadOnlyCollection<string> Availability,
        string YearsOfExperience,
        string CredentialsDocument,
        string GovernmentID,
        decimal HourlyRate,
        string Bio,
        bool IsVerified,
        string TimeZoneId,
        string UserProfileId,
        string? ProfilePictureURL
    );

    public record CreateTutorRequest(
        Gender Gender,
        DateOnly DateOfBirth,
        Location Location,
        string YearsOfExperience,
        string CredentialsDocument,
        string GovernmentID,
        decimal HourlyRate,
        string Bio,
        string TimeZoneId,
        string[] Qualifications,
        string[] Subjects,
        string[] Availability,
        string? ProfilePictureURL
    );

    public record UpdateTutorRequest(
        Gender? Gender = null,
        DateOnly? DateOfBirth = null,
        Location? Location = null,
        string? YearsOfExperience = null,
        string? CredentialsDocument = null,
        string? GovernmentID = null,
        decimal? HourlyRate = null,
        string? Bio = null,
        string? TimeZoneId = null,
        string[]? Qualifications = null,
        string[]? Subjects = null,
        string[]? Availability = null,
        string? ProfilePictureURL = null
    );
}

