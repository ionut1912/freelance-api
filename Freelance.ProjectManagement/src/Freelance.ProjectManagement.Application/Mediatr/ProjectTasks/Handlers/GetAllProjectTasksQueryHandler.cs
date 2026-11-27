using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public sealed class GetAllProjectTasksQueryHandler(IProjectTaskRepository projectTaskRepository)
{
    public async Task<List<ProjectTaskDto>> Handle(GetAllProjectTasksQuery query, CancellationToken cancellationToken)
    {
        var projectTasks = await projectTaskRepository.GetAllAsync(cancellationToken, pt => pt.TimeLogs);
        return projectTasks.ToDtos();
    }
}
