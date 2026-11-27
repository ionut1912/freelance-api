using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public sealed class UpdateTimeLogCommandHandler(
    IProjectTaskRepository projectTaskRepository,
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateTimeLogCommandHandler> logger)
{
    public async Task Handle(UpdateTimeLogCommand command, CancellationToken cancellationToken)
    {
        var projectTask = await projectTaskRepository.GetByIdWithTrackingAsync(command.TaskId, cancellationToken, pt => pt.TimeLogs)
                          ?? throw new ProjectTaskNotFoundException($"Project task with id {command.TaskId} was not found");
        // A task the freelancer is not assigned to is reported as missing.
        if (!await projectRepository.IsAssignedAsync(projectTask, command.FreelancerId, cancellationToken))
            throw new ProjectTaskNotFoundException($"Project task with id {command.TaskId} was not found");

        // Throws TimeLogNotFoundException when the time log does not belong to the task.
        projectTask.UpdateTimeLog(command.Id, command.StartTime, command.EndTime);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Time log {TimeLogId} was updated", command.Id);
    }
}
