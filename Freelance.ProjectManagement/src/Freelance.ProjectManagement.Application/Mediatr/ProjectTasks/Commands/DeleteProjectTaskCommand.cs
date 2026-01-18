using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;

public record DeleteProjectTaskCommand(Guid Id) : IRequest
{
}
