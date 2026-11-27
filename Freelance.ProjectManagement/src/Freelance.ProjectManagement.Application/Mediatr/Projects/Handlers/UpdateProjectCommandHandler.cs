using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public sealed class UpdateProjectCommandHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProjectCommandHandler> logger)
{
    public async Task Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        // Tracked load: EF change tracking produces the minimal UPDATE statements.
        var project = await projectRepository.GetByIdWithTrackingAsync(command.Id, cancellationToken, p => p.Technologies)
                      ?? throw new ProjectNotFoundException($"Project with id {command.Id} was not found");

        project.Update(command.Title, command.Description, command.Deadline, command.Amount, command.Currency);
        project.UpdateTechnologies(command.Technologies.Select(ProjectTechnology.Create));

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Project {ProjectId} was updated", command.Id);
    }
}
