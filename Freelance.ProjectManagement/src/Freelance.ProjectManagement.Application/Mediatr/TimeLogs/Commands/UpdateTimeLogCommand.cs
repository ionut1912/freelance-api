using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

public record UpdateTimeLogCommand(Guid Id,Guid TaskId,DateTime StartTime, DateTime EndTime) : IRequest
{
}
