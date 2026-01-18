using Freelance.ProjectManagement.Domain.Entities;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;

public record CreateProjectTaskCommand(Guid ProjectId, string Title, string Description, string Status, string Priority) : IRequest<ProjectTask>
{
}
