using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public sealed class CreateProjectCommandHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateProjectCommandHandler> logger)
{
    public async Task<ProjectDto> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = Project.Create(command.Title, command.Description, command.Deadline, command.Amount, command.Currency);
        project.AssignClient(command.ClientId);
        project.AddTechnologies(command.Technologies.Select(ProjectTechnology.Create));

        await projectRepository.AddAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Project {ProjectId} was created", project.Id);
        return project.ToDto();
    }
}
