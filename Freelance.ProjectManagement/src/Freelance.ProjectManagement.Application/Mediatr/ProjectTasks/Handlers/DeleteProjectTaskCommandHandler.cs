using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public sealed class DeleteProjectTaskCommandHandler(
    IProjectTaskRepository projectTaskRepository,
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteProjectTaskCommandHandler> logger)
{
    public async Task Handle(DeleteProjectTaskCommand command, CancellationToken cancellationToken)
    {
        var projectTask = await projectTaskRepository.GetByIdAsync(command.Id, cancellationToken)
                          ?? throw new ProjectTaskNotFoundException($"Project task with id {command.Id} was not found");

        var project = await projectRepository.GetByIdAsync(projectTask.ProjectId, cancellationToken);
        if (project?.ClientId != command.ClientId)
            throw new ProjectTaskNotFoundException($"Project task with id {command.Id} was not found");

        projectTaskRepository.Delete(projectTask);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Project task {TaskId} was deleted", command.Id);
    }
}
