using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.Application.Mappings;

public static class TaskMapping
{
    extension(ProjectTask task)
    {
        public ProjectTaskDto ToDto() => new(
            task.Id,
            task.ProjectId,
            task.Title,
            task.Description,
            task.TimeLogs.ToDtos(),
            task.FreelacerId,
            task.Status.Value,
            task.Priority.Value);
    }

    extension(IEnumerable<ProjectTask> tasks)
    {
        public List<ProjectTaskDto> ToDtos() => [.. tasks.Select(t => t.ToDto())];
    }

    extension(CreateProjectTaskRequest request)
    {
        public CreateProjectTaskCommand ToCreateCommand(Guid accountId) =>
            new(request.ProjectId, request.Title, request.Description, request.Status, request.Priority,
                accountId);
    }

    extension(UpdateProjectTaskReuqest request)
    {
        public UpdateProjectTaskCommand ToUpdateCommand(Guid id, Guid accountId) =>
            new(id, request.ProjectId, request.Title, request.Description, request.Status, request.Priority,
                accountId);
    }
}
