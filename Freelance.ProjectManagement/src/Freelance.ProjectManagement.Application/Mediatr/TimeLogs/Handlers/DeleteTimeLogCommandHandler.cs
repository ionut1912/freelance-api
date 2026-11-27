using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public sealed class DeleteTimeLogCommandHandler(
    ITimeLogRepository timeLogRepository,
    IProjectTaskRepository projectTaskRepository,
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteTimeLogCommandHandler> logger)
{
    public async Task Handle(DeleteTimeLogCommand command, CancellationToken cancellationToken)
    {
        var timeLog = await timeLogRepository.GetByIdAsync(command.Id, cancellationToken)
                      ?? throw new TimeLogNotFoundException($"TimeLog with Id {command.Id} was not found");
        var projectTask = await projectTaskRepository.GetByIdAsync(timeLog.TaskId, cancellationToken);
        if (projectTask is null ||
            !await projectRepository.IsAssignedAsync(projectTask, command.FreelancerId, cancellationToken))
            throw new TimeLogNotFoundException($"TimeLog with Id {command.Id} was not found");

        timeLogRepository.Delete(timeLog);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Time log {TimeLogId} was deleted", command.Id);
    }
}
