
using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.Application.Mappings;

public static class TaskMapping
{
    public static ProjectTaskDto ToDto(this ProjectTask task)
    {
        return new ProjectTaskDto(
            task.Id,
            task.ProjectId,
            task.Title,
            task.Description,

            task.TimeLogs.ToDtos(),
            task.FreelacerId,
            task.Status.Value,
            task.Priority.Value);
    }

    public static List<ProjectTaskDto> ToDtos(this IEnumerable<ProjectTask> tasks)
    {
        return [.. tasks.Select(t => t.ToDto())];
    }

    public static CreateProjectTaskCommand ToCreateCommand(this CreateProjectTaskRequest request)
    {
        return new CreateProjectTaskCommand(request.ProjectId, request.Title, request.Description, request.Status, request.Priority);
    }

    public static UpdateProjectTaskCommand ToUpdateCommand(this UpdateProjectTaskReuqest request,Guid Id)
    {
        return new UpdateProjectTaskCommand(Id,request.ProjectId,request.Title, request.Description, request.Status, request.Priority);
    }
}
