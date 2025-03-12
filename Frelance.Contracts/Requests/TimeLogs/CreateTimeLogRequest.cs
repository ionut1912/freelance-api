using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.TimeLogs;

[UsedImplicitly]
public record CreateTimeLogRequest(string TaskTitle, DateTime StartTime, DateTime EndTime);