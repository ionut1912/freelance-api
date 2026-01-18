using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProjectCommandHandler> _logger;

    public CreateProjectCommandHandler(IProjectRepository repository, IUnitOfWork unitOfWork, ILogger<CreateProjectCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectRepository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;

    }

    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Project.Create(request.Title, request.Description, request.Deadline, request.Amount, request.Currency);
        var projectTechnologies = request.Technologies
            .Select(t => new ProjectTechnology(t))
            .ToList();
        project.AddTechnologies(projectTechnologies);
        await _projectRepository.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Project with ID: {ProjectId} has been created.", project.Id);
        return project;
    }
}
