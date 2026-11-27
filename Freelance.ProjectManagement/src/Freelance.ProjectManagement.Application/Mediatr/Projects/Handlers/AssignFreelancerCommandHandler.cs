using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public sealed class AssignFreelancerCommandHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<AssignFreelancerCommandHandler> logger)
{
    public async Task Handle(AssignFreelancerCommand command, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdWithTrackingAsync(command.ProjectId, cancellationToken);
        // Someone else's project is reported as missing, so ids cannot be probed.
        if (project?.ClientId != command.ClientId)
            throw new ProjectNotFoundException($"Project with id {command.ProjectId} was not found");

        project.AssignFreelancer(command.FreelancerId);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Freelancer {FreelancerId} was assigned to project {ProjectId}", command.FreelancerId,
            command.ProjectId);
    }
}
