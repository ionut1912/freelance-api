using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

public record DeleteTimeLogCommand(Guid Id) : IRequest
{
}
