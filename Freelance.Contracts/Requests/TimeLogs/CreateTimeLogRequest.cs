using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.TimeLogs;

[UsedImplicitly]
public record CreateTimeLogRequest(string TaskTitle, DateTime StartTime, DateTime EndTime);