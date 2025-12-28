using HolookorBackend.Core.Domain.Enums;
using TimeZoneConverter;


namespace HolookorBackend.Core.Domain.Entities
{
    public class Tutor : AuditableEntities
    {
        public ICollection<string> Qualifications { get; private set; } = new List<string>();
        public ICollection<string> Subjects { get; private set; } = new List<string>();
        public Gender Gender { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public Location Location { get; private set; } = default!;
        public ICollection<string> Availability { get; private set; } = new List<string>();
        public string YearsOfExperience { get; private set; } = default!;
        public string? ProfilePictureURL { get; private set; }
        public string TimeZoneId { get; private set; } = default!;
        public string CredentialsDocument { get; private set; } = default!;
        public string GovernmentID { get; private set; } = default!;
        public decimal HourlyRate { get; private set; }
        public bool IsVerified { get; private set; } = false;
        public string Bio { get; private set; } = default!;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedOn { get; private set; }
        public string UserProfileId { get; private set; } = default!;
        public virtual UserProfile Profile { get; private set; } = default!;
        public virtual ICollection<TutorReview> Reviews { get; private set; } = new List<TutorReview>();

        private Tutor() { }

        public Tutor(
            Gender gender,
            DateOnly dateOfBirth,
            Location location,
            string yearsOfExperience,
            string credentialsDocument,
            string governmentID,
            decimal hourlyRate,
            string bio,
            string timeZoneId)
        {
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Location = location ?? throw new ArgumentNullException(nameof(location));
            YearsOfExperience = yearsOfExperience;
            CredentialsDocument = credentialsDocument;
            GovernmentID = governmentID;
            HourlyRate = hourlyRate >= 0 ? hourlyRate : throw new ArgumentException("Hourly rate must be >= 0", nameof(hourlyRate));
            Bio = bio;
            SetTimeZone(timeZoneId);
        }

        //public void SetTimeZone(string timeZoneId)
        //{
        //    if (!TimeZoneInfo.GetSystemTimeZones().Any(t => t.Id == timeZoneId))
        //        throw new ArgumentException("Invalid TimeZone", nameof(timeZoneId));
        //    TimeZoneId = timeZoneId;
        //}
        public void SetTimeZone(string timeZoneId)
        {
            try
            {
                var windowsTz = TZConvert.IanaToWindows(timeZoneId);
                TimeZoneId = windowsTz;
            }
            catch
            {
                throw new ArgumentException("Invalid TimeZone");
            }
        }

        public void AddQualification(string qualification)
        {
            if (!string.IsNullOrWhiteSpace(qualification) && !Qualifications.Contains(qualification))
                Qualifications.Add(qualification);
        }

        public void AddSubject(string subject)
        {
            if (!string.IsNullOrWhiteSpace(subject) && !Subjects.Contains(subject))
                Subjects.Add(subject);
        }

        public void AddAvailability(string availability)
        {
            if (!string.IsNullOrWhiteSpace(availability) && !Availability.Contains(availability))
                Availability.Add(availability);
        }

        public void Verify()
        {
            IsVerified = true;
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedOn = DateTime.UtcNow;
        }

        public void UpdateProfilePicture(string url)
        {
            ProfilePictureURL = url;
        }
        public void AssignProfile(string userProfileId)
        {
            if (string.IsNullOrWhiteSpace(userProfileId))
                throw new ArgumentException("UserProfileId cannot be empty");

            UserProfileId = userProfileId;
        }
        public void UpdateBio(string bio)
        {
            if (!string.IsNullOrWhiteSpace(bio))
                Bio = bio;
        }
    }
}
