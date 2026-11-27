using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public sealed class CreateProjectTaskCommandHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateProjectTaskCommandHandler> logger)
{
    public async Task<ProjectTaskDto> Handle(CreateProjectTaskCommand command, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdWithTrackingAsync(command.ProjectId, cancellationToken)
                      ?? throw new ProjectNotFoundException($"Project with Id {command.ProjectId} was not found");
        // Only the owning client and the assigned freelancer may touch the project's tasks.
        if (!project.IsMember(command.AccountId))
            throw new ProjectNotFoundException($"Project with Id {command.ProjectId} was not found");

        var task = ProjectTask.Create(project.Id, command.Title, command.Description,
            ProjectTaskStatus.FromName(command.Status), ProjectTaskPriority.FromName(command.Priority));

        // The project is the aggregate root: adding through it lets EF set the relationship and insert the task.
        project.AddTask(task);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Task {TaskId} was created for project {ProjectId}", task.Id, project.Id);
        return task.ToDto();
    }
}
