namespace Frelance.Infrastructure.Entities;

public class TimeLog
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public ProjectTask? Task { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateOnly Date { get; set; }
    public int TotalHours { get; set; }
}