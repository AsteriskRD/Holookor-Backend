namespace HolookorBackend.Core.Domain.Entities
{
    public class UserProfile : AuditableEntities
    {
        private UserProfile() { }

        public UserProfile(
            string firstName,
            string lastName,
            string role,
            string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            PhoneNumber = phoneNumber;
            IsActive = true;
        }

        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Role { get; private set; } = default!;
        public bool IsActive { get; private set; }
        public bool IsEmailVerified { get;  set; }
        public string PhoneNumber { get; private set; } = default!;
        public virtual User Users { get; private set; } = default!;
    
    }

}
