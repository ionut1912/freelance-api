using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.TimeLogs;

[UsedImplicitly]
public record UpdateTimeLogRequest(string? TaskTitle, DateTime? StartTime, DateTime? EndTime, int? TotalHours);