using HolookorBackend.Core.Domain.Enums;

namespace HolookorBackend.Core.Application.DTOs;

    public class StudentSessionDto
    {
        public string SessionId { get; set; } = default!;
        public string TutorName { get; set; } = default!;
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }

        public bool CanJoinClass { get; set; }
    }

public class TutorSessionDto
{
    public string SessionId { get; set; } = default!;
    public string StudentName { get; set; } = default!;
    public string StudentId { get; set; } = default!;
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }

    public bool CanJoinClass { get; set; }
}

//public bool CanJoinClass(Session session)
//    {
//        if (session.Status != SessionStatus.Confirmed)
//            return false;

//        var nowUtc = DateTime.UtcNow;

//        // Allow joining 10 mins before, lock 10 mins after
//        return nowUtc >= session.StartTimeUtc.AddMinutes(-10)
//            && nowUtc <= session.EndTimeUtc.AddMinutes(10);
//    }

public record BookSessionRequest(
    string TutorId,
    DayOfWeek DayOfWeek,
    TimeSpan TutorStartTime,
    int DurationHours,
    string Subject,
    string? Topic,
    string? Note
);
