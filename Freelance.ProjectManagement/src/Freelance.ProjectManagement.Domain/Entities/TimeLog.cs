using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.Entities;

public class TimeLog : Entity
{
    private TimeLog() //for EF core
    {

    }

    private TimeLog(Guid taskId, DateTime startTime, DateTime endTime)
    {
        TaskId = taskId;
        StartTime = startTime.Kind == DateTimeKind.Utc
        ? startTime
        : DateTime.SpecifyKind(startTime, DateTimeKind.Utc);
        EndTime = endTime.Kind == DateTimeKind.Utc
        ? endTime
        : DateTime.SpecifyKind(endTime, DateTimeKind.Utc); ;
        TotalHours = endTime.Hour - startTime.Hour; ;
    }

    public Guid TaskId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int TotalHours { get; private set; }

    public static TimeLog Create(Guid taskId, DateTime startTime, DateTime endTime)
    {
        return new TimeLog(taskId, startTime, endTime);
    }

    public void Update(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime.Kind == DateTimeKind.Utc
         ? startTime
         : DateTime.SpecifyKind(startTime, DateTimeKind.Utc);
        EndTime = endTime.Kind == DateTimeKind.Utc
        ? endTime
        : DateTime.SpecifyKind(endTime, DateTimeKind.Utc); ;
        TotalHours = endTime.Hour - startTime.Hour;
    }


}
