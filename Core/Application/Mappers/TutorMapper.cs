using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Mappers
{
    public static class TutorMapper
    {
        public static TutorDto Map(Tutor t)
        {
            return new TutorDto(
                t.Id,
                t.Gender,
                t.DateOfBirth,
                t.Location,
                t.Qualifications.ToList(),
                t.Subjects.ToList(),
                t.Availability.ToList(),
                t.YearsOfExperience,
                t.CredentialsDocument,
                t.GovernmentID,
                t.HourlyRate,
                t.Bio,
                t.IsVerified,
                t.TimeZoneId,
                t.UserProfileId,
                t.ProfilePictureURL
            );
        }
    }
}
