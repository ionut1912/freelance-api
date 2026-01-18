using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public class GettAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GettAllProjectsQueryHandler> _logger;

    public GettAllProjectsQueryHandler(IProjectRepository projectRepository, ILogger<GettAllProjectsQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectRepository = projectRepository;
        _logger = logger;
    }


    public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(cancellationToken, p => p.Tasks, p => p.Technologies);
        var projectDtos = projects.ToDtos();
        _logger.LogInformation("Found projects {projectDtos}", JsonSerializer.Serialize(projectDtos, new JsonSerializerOptions { WriteIndented = true }));
        return projectDtos;
    }
}
