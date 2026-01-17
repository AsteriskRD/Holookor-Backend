namespace HolookorBackend.Core.Domain.Entities
{
    public class Payment : AuditableEntities
    {
        public string SessionId { get; private set; } = default!;
        public decimal Amount { get; private set; }
        public string Provider { get; private set; } = "Stripe";
        public string ProviderReference { get; private set; } = default!;
        public bool IsSuccessful { get; private set; }
        public virtual Session Session { get; private set; } = default!;
        public SessionBillingInfo BillingInfo { get; private set; } = default!;

        private Payment() { }

        public Payment(string sessionId, decimal amount,string provider, string providerReference, SessionBillingInfo billingInfo)
        {
            SessionId = sessionId;
            Amount = amount;
            Provider = provider;
            ProviderReference = providerReference;
            BillingInfo = billingInfo;
            IsSuccessful = true;
        }

        public void MarkSuccessful()
        {
            IsSuccessful = true;
        }
    }

}
