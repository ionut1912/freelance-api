namespace Frelance.Contracts.Requests.TimeLogs;

public class UpdateTimeLogRequest
{
    public string? TaskTitle { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? TotalHours { get; set; }
};