namespace Frelance.Contracts.Requests.TimeLogs;

public record UpdateTimeLogRequest(string TaskTitle, DateTime StartTime, DateTime EndTime, int TotalHours,DateOnly Date);