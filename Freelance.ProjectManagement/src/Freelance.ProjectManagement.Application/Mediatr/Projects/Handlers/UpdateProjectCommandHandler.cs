using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
using System.Text.Json;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProjectCommandHandler> _logger;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, ILogger<UpdateProjectCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdWithTrackingAsync(request.Id, cancellationToken, p => p.Tasks, p => p.Technologies);
        if (project == null)
        {
            _logger.LogError("We can't update project,because project with Id {Id} was not found", request.Id);
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");
        }

        project.Update(request.Title, request.Description, request.Deadline, request.Amount, request.Currency);

        var projectTechnologies = request.Technologies
            .Select(t => new ProjectTechnology(t))
            .ToList();

        project.UpdateTechnologies(projectTechnologies);

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Project with Id {Id} was updated successfully,{newProject}", request.Id, JsonSerializer.Serialize(project, new JsonSerializerOptions { WriteIndented = true }));
        return Unit.Value;
    }
}