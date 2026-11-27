using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.Entities;

public sealed class TimeLog : Entity
{
    private TimeLog() // for EF Core
    {
    }

    private TimeLog(Guid taskId, DateTime startTime, DateTime endTime)
    {
        TaskId = taskId;
        SetInterval(startTime, endTime);
    }

    public Guid TaskId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    /// <summary>Whole hours logged between <see cref="StartTime"/> and <see cref="EndTime"/>.</summary>
    public int TotalHours { get; private set; }

    public static TimeLog Create(Guid taskId, DateTime startTime, DateTime endTime) => new(taskId, startTime, endTime);

    public void Update(DateTime startTime, DateTime endTime)
    {
        SetInterval(startTime, endTime);
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetInterval(DateTime startTime, DateTime endTime)
    {
        var start = AsUtc(startTime);
        var end = AsUtc(endTime);
        if (end < start)
            throw new ArgumentException("End time must be after start time.", nameof(endTime));

        StartTime = start;
        EndTime = end;
        // Previously computed as end.Hour - start.Hour, which was wrong across days and ignored minutes.
        TotalHours = (int)(end - start).TotalHours;
    }

    private static DateTime AsUtc(DateTime value) =>
        value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);
}
