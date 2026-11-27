using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public sealed class GetProjectTaskByIdQueryHandler(IProjectTaskRepository projectTaskRepository)
{
    public async Task<ProjectTaskDto> Handle(GetProjectTaskByIdQuery query, CancellationToken cancellationToken)
    {
        var projectTask = await projectTaskRepository.GetByIdAsync(query.Id, cancellationToken, pt => pt.TimeLogs)
                          ?? throw new ProjectTaskNotFoundException($"Project task with id {query.Id} was not found");

        return projectTask.ToDto();
    }
}
