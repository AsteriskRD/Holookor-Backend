namespace HolookorBackend.Core.Domain.Entities
{
    public class EmailVerification : AuditableEntities
    {
        public string UserProfileId { get; private set; } = default!;
        public string Code { get; private set; } = default!;
        public bool IsUsed { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        private EmailVerification() { }

        public EmailVerification(string userProfileId, string code, int expiryMinutes = 15)
        {
            UserProfileId = userProfileId;
            Code = code;
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
            IsUsed = false;
        }

        public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

        public void MarkUsed()
        {
            IsUsed = true;
            DateUpdated = DateTime.UtcNow;
        }
    }
}
