using Freelance.ProjectManagement.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;

public record GetAllProjectTasksQuery : IRequest<List<ProjectTaskDto>>
{
}
