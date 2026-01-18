using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;

public record class UpdateProjectTaskCommand(Guid Id, Guid ProjectId, string Title, string Description, string Status, string Priority) : IRequest
{
}
