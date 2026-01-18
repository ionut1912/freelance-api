using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{

    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetProjectByIdQueryHandler> _logger;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository, ILogger<GetProjectByIdQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken, p => p.Tasks, p => p.Technologies);

        if (project == null)
        {
            _logger.LogError("Project with Id {Id} was not found", request.Id);
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");
        }

        var projectDto = project.ToDto();
        _logger.LogInformation("Found project {projectDto}", JsonSerializer.Serialize(projectDto, new JsonSerializerOptions { WriteIndented = true }));
        return projectDto;
    }
}
