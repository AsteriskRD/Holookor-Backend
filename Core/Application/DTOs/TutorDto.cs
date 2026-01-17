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
        IReadOnlyCollection<AvailabilityDto> Availability,
        int YearsOfExperience,
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
        int YearsOfExperience,
        string CredentialsDocument,
        string GovernmentID,
        decimal HourlyRate,
        string Bio,
        string TimeZoneId,
        string[] Qualifications,
        string[] Subjects,
        IReadOnlyCollection<AvailabilityDto> Availability,
        string? ProfilePictureURL
    );

    public record UpdateTutorRequest(
        Gender? Gender = null,
        DateOnly? DateOfBirth = null,
        Location? Location = null,
        int? YearsOfExperience = null,
        string? CredentialsDocument = null,
        string? GovernmentID = null,
        decimal? HourlyRate = null,
        string? Bio = null,
        string? TimeZoneId = null,
        string[]? Qualifications = null,
        string[]? Subjects = null,
        IReadOnlyCollection<AvailabilityDto>? Availability = null,
        string? ProfilePictureURL = null
    );

    public record TutorSearchDto(
        string TutorId,
        string FullName,
        Gender Gender,
        decimal HourlyRate,
        bool IsVerified,
        bool IsAvailable,
        double AverageRating,
        int ReviewCount,
        string? ProfilePictureUrl,
        IReadOnlyList<string> Subjects
    );


    public class TutorSearchRequestDto
    {
        public string? Subject { get; set; }
        public Gender? Gender { get; set; }
        public decimal? MinRate { get; set; }
        public decimal? MaxRate { get; set; }
        public int? MinRating { get; set; }
        public string? Availability { get; set; }
        public string? Name { get; set; }
        public TutorSortOption? SortBy { get; set; } = TutorSortOption.Newest;

    }

    public record TutorSearchResponseDto(
    string TutorId,
    string FullName,
    Gender Gender,
    decimal HourlyRate,
    bool IsAvailable,
    bool IsVerified,
    double AverageRating,
    int ReviewCount,
    IReadOnlyCollection<string> Subjects
    );

    public record AvailabilityDto(
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime
    );
}
