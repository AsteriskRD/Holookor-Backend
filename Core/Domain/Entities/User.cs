namespace HolookorBackend.Core.Domain.Entities
{
    public class User : AuditableEntities
    {
        public string Email { get; set; } = default!;
        public string PassWord { get; set; } = default!;

        public string UserProfileId { get; set; } = default!;
        public virtual UserProfile UserProfile { get; set; } = default!;
    }
}
