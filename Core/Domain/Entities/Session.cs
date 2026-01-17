using HolookorBackend.Core.Domain.Enums;

namespace HolookorBackend.Core.Domain.Entities
{
    public class Session : AuditableEntities
    {
        public string TutorId { get; private set; } = default!;
        public string StudentId { get; private set; } = default!;

        public DateTime StartTimeUtc { get; private set; }
        public DateTime EndTimeUtc { get; private set; }

        public string Subject { get; private set; } = default!;
        public string? Topic { get; private set; }
        public string? Notes { get; private set; }

        public decimal HourlyRate { get; private set; }
        public decimal ServiceFee { get; private set; }
        public decimal Tax { get; private set; }
        public decimal TotalAmount { get; private set; }

        public SessionStatus Status { get; private set; }

        public bool IsDeleted { get; private set; }

        public virtual Tutor Tutor { get; private set; } = default!;
        public virtual Student Student { get; private set; } = default!;

        private Session() { }

        public Session(
            string tutorId,
            string studentId,
            DateTime startUtc,
            DateTime endUtc,
            string subject,
            decimal hourlyRate,
            decimal serviceFee,
            decimal tax,
            string? topic = null,
            string? notes = null)
        {
            TutorId = tutorId;
            StudentId = studentId;
            StartTimeUtc = startUtc;
            EndTimeUtc = endUtc;
            Subject = subject;
            Topic = topic;
            Notes = notes;
            HourlyRate = hourlyRate;
            ServiceFee = serviceFee;
            Tax = tax;


            var durationHours = (decimal)(EndTimeUtc - StartTimeUtc).TotalHours;
            TotalAmount = (HourlyRate * durationHours) + ServiceFee + Tax;
            Status = SessionStatus.PendingPayment;
        }

        public void MarkPaid()
        {
            if (Status != SessionStatus.PendingPayment)
                throw new InvalidOperationException("Session not awaiting payment");
            Status = SessionStatus.Paid;
        }

        public void Confirm()
        {
            if (Status != SessionStatus.Paid)
                throw new InvalidOperationException("Session must be paid first");
            Status = SessionStatus.Confirmed;
        }

        public bool CanJoin(DateTime nowUtc)
        {
            return Status == SessionStatus.Confirmed
                && nowUtc >= StartTimeUtc.AddMinutes(-5)
                && nowUtc <= EndTimeUtc;
        }

        public void Cancel()
        {
            Status = SessionStatus.Cancelled;
        }

        public void Complete()
        {
            Status = SessionStatus.Completed;
        }
    }
}
