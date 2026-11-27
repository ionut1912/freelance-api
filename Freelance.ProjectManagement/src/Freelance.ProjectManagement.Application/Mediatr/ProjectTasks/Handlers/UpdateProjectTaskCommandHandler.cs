using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public sealed class UpdateProjectTaskCommandHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProjectTaskCommandHandler> logger)
{
    public async Task Handle(UpdateProjectTaskCommand command, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdWithTrackingAsync(command.ProjectId, cancellationToken, p => p.Tasks)
                      ?? throw new ProjectNotFoundException($"Project with Id {command.ProjectId} was not found");

        // Throws ProjectTaskNotFoundException when the task does not belong to the project.
        project.UpdateTask(command.Id, command.Title, command.Description,
            ProjectTaskStatus.FromName(command.Status), ProjectTaskPriority.FromName(command.Priority));

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Project task {TaskId} was updated", command.Id);
    }
}
