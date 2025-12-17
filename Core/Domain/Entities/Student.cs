using HolookorBackend.Core.Domain.Enums;

namespace HolookorBackend.Core.Domain.Entities
{
    public class Student : AuditableEntities
    {
        private const int ChildIdLength = 6;
        public string SchoolLevel { get; private set; } = default!;
        public DateOnly DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public WeeklyAvailability WeeklyAvailability { get; private set; }
        public Location Location { get; private set; } = default!;
        public string ChildID { get; private set; } = default!;
        public string TimeZoneId { get; private set; } = default!;
        public PreferredClassTime PreferredClassTime { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public string UserProfileId { get; private set; } = default!;
        public virtual UserProfile Profile { get; private set; } = default!;
        public ICollection<string> SubjectOfInterest { get; private set; } = new List<string>();
        public string? ParentId { get; private set; }
        public virtual Parent? Parent { get; private set; }


        private Student() { }
        public Student(
            string schoolLevel,
            DateOnly dateOfBirth,
            Gender gender,
            WeeklyAvailability weeklyAvailability,
            Location location,
            string timeZoneId,
            PreferredClassTime preferredClassTime,
            string[] subjectsOfInterest)
        {
            SchoolLevel = schoolLevel;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            WeeklyAvailability = weeklyAvailability;
            Location = location;
            SetTimeZone(timeZoneId);
            GenerateChildId();
            PreferredClassTime = preferredClassTime;

            if (subjectsOfInterest != null)
            {
                foreach (var subject in subjectsOfInterest)
                {
                    AddSubjectOfInterest(subject);
                }
            }
        }


        public void SetTimeZone(string timeZoneId)
        {
            if (!TimeZoneInfo.GetSystemTimeZones().Any(t => t.Id == timeZoneId))
                throw new ArgumentException("Invalid TimeZone");
            TimeZoneId = timeZoneId;
        }

        public void AddSubjectOfInterest(string subject)
        {
            if (!SubjectOfInterest.Contains(subject))
                SubjectOfInterest.Add(subject);
        }

        public void UpdatePreferredClassTime(PreferredClassTime preferredClassTime)
        {
            PreferredClassTime = preferredClassTime;
        }

        public void UpdateLocation(Location location)
        {
            Location = location;
        }

        public void AssignProfile(string userProfileId)
        {
            if (string.IsNullOrWhiteSpace(userProfileId))
                throw new ArgumentException("UserProfileId cannot be empty");

            UserProfileId = userProfileId;
        }

        private void GenerateChildId()
        {
            ChildID = ChildIdGenerator.Generate(ChildIdLength);
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public static class ChildIdGenerator
        {
            private static readonly char[] AllowedChars =
                "ABCDEFGHJKMNPQRSTUVWXYZ23456789".ToCharArray();

            public static string Generate(int length)
            {
                var random = Random.Shared;
                return new string(
                    Enumerable.Range(0, length)
                        .Select(_ => AllowedChars[random.Next(AllowedChars.Length)])
                        .ToArray()
                );
            }
        }

        public void AssignParent(string parentId)
        {
            ParentId = parentId;
        }


    }
}
