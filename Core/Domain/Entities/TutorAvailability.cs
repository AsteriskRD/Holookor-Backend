using HolookorBackend.Core.Application.Exceptions.HolookorBackend.Core.Application.Exceptions;

namespace HolookorBackend.Core.Domain.Entities
{
    public class TutorAvailability : AuditableEntities
    {
        public DayOfWeek DayOfWeek { get;private set; }  
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        private TutorAvailability() { }
        public TutorAvailability(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
        {
            if (startTime >= endTime) 
                throw new DomainException("Start time must be before endtime");

            DayOfWeek = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
