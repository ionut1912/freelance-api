using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public class GetAllProjectTasksQueryHandler : IRequestHandler<GetAllProjectTasksQuery, List<ProjectTaskDto>>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly ILogger<GetAllProjectTasksQueryHandler> _logger;

    public GetAllProjectTasksQueryHandler(IProjectTaskRepository projectTaskRepository,ILogger<GetAllProjectTasksQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectTaskRepository, nameof(projectTaskRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectTaskRepository = projectTaskRepository;
        _logger = logger;
    }

    public async Task<List<ProjectTaskDto>> Handle(GetAllProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var projectTask = await _projectTaskRepository.GetAllAsync(cancellationToken,pt=>pt.TimeLogs);
        var projectTaskDtos = projectTask.ToDtos();
        _logger.LogInformation("Foudn projectTask {projectTaskDtos}", JsonSerializer.Serialize(projectTaskDtos, new JsonSerializerOptions { WriteIndented = true }));
        return projectTaskDtos;
    }
}
