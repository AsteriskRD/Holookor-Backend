namespace HolookorBackend.Core.Domain.Entities
{
    public class TutorReview : AuditableEntities
    {
        public string TutorId { get; set; } = default!;
        public string StudentId { get; set; } = default!;
        public int Rating { get; set; }
        public string Comment { get; set; } = default!;
        public DateTime CreatedOn { get; set; }

        public virtual Tutor Tutor { get; set; } = default!;
        public virtual Student Student { get; set; } = default!;
    }
}
