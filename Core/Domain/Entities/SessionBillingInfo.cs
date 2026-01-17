namespace HolookorBackend.Core.Domain.Entities
{
    public class SessionBillingInfo
    {
        public string Email { get; private set; } = default!;
        public string Country { get; private set; } = default!;
        public string TimeZoneId { get; private set; } = default!;

        private SessionBillingInfo() { }

        public SessionBillingInfo(string email, string country, string timeZoneId)
        {
            Email = email;
            Country = country;
            TimeZoneId = timeZoneId;
        }
    }
}
