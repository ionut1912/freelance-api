using Freelance.ProjectManagement.Domain.Entities;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

public record CreateTimeLogCommand(Guid TaskId, DateTime StartTime, DateTime EndTime) : IRequest<TimeLog>
{
}
