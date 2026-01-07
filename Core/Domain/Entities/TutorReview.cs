namespace HolookorBackend.Core.Domain.Entities
{
    public class TutorReview : AuditableEntities
    {
        public string TutorId { get; private set; } = default!;
        public string StudentId { get; private set; } = default!;
        public int Rating { get; private set; } 
        public string Comment { get; private set; } = default!;
        public DateTime CreatedOn { get; private set; }

        public virtual Tutor Tutor { get; private set; } = default!;
        public virtual Student Student { get; private set; } = default!;

        private TutorReview() { }

        public TutorReview(string tutorId, string studentId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            TutorId = tutorId;
            StudentId = studentId;
            Rating = rating;
            Comment = comment;
            CreatedOn = DateTime.UtcNow;
        }
    }

}
