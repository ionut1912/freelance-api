namespace Frelance.Contracts.Requests.TimeLogs;

public record CreateTimeLogRequest(string TaskTitle, DateTime StartTime, DateTime EndTime);