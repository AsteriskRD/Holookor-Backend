namespace HolookorBackend.Core.Application.DTOs
{
    public record TutorReviewDto(
     string Id,
    string StudentId,
    int Rating,
    string Comment,
    DateTime CreatedOn
    );

    public class CreateTutorReviewRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = default!;
    }
}
