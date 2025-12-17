using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Mappers
{
    public static class StudentMapper
    {
        public static StudentDto Map(Student s)
        {
            return new StudentDto(
                s.Id,
                s.SchoolLevel,
                s.DateOfBirth,
                s.Gender,
                s.WeeklyAvailability,
                s.Location,
                s.TimeZoneId,
                s.PreferredClassTime,
                s.SubjectOfInterest.ToList(),
                s.UserProfileId,
                s.ChildID
            );
        }
    }
}
