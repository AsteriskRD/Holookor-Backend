namespace HolookorBackend.Core.Application.DTOs;

public class BookingSummaryDto
{
    public string SessionId { get; set; } = default!;
    public string TutorName { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public int SessionCount { get; set; }
    public string? Topic { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }

    public decimal BasePrice { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal Tax { get; set; }
    public decimal TotalAmount { get; set; }
}
