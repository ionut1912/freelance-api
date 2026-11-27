using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public sealed class CreateTimeLogCommandHandler(
    IProjectTaskRepository projectTaskRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateTimeLogCommandHandler> logger)
{
    public async Task<TimeLogDto> Handle(CreateTimeLogCommand command, CancellationToken cancellationToken)
    {
        var projectTask = await projectTaskRepository.GetByIdWithTrackingAsync(command.TaskId, cancellationToken)
                          ?? throw new ProjectTaskNotFoundException($"Project task with id {command.TaskId} was not found");

        var timeLog = TimeLog.Create(command.TaskId, command.StartTime, command.EndTime);
        projectTask.AddTimeLog(timeLog);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Time log {TimeLogId} was created for task {TaskId}", timeLog.Id, command.TaskId);
        return timeLog.ToDto();
    }
}
