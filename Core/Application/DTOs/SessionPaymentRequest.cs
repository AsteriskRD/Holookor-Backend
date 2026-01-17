namespace HolookorBackend.Core.Application.DTOs
{
    public class SessionPaymentRequest
    {
        public string Email { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string TimeZoneId { get; set; } = default!;
        public string PaymentMethodToken { get; set; } = default!;
    }

}
