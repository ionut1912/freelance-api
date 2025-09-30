using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.TimeLogs;

[UsedImplicitly]
public record UpdateTimeLogRequest(string? TaskTitle, DateTime? StartTime, DateTime? EndTime, int? TotalHours);