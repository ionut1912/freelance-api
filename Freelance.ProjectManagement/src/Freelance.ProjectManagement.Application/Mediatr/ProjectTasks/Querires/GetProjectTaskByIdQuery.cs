using Freelance.ProjectManagement.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;

public record GetProjectTaskByIdQuery(Guid Id) : IRequest<ProjectTaskDto>
{
}
