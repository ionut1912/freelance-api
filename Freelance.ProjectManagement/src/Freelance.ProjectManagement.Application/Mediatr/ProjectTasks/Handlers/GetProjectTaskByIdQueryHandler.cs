using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public class GetProjectTaskByIdQueryHandler : IRequestHandler<GetProjectTaskByIdQuery, ProjectTaskDto>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly ILogger<GetProjectTaskByIdQueryHandler> _logger;

    public GetProjectTaskByIdQueryHandler(IProjectTaskRepository projectTaskRepository,ILogger<GetProjectTaskByIdQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectTaskRepository, nameof(projectTaskRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectTaskRepository = projectTaskRepository;
        _logger = logger;
    }

    public async Task<ProjectTaskDto> Handle(GetProjectTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var projectTask = await _projectTaskRepository.GetByIdAsync(request.Id, cancellationToken, pt => pt.TimeLogs);
        if (projectTask == null)
        {
            _logger.LogError("Project Task with Id {Id} was not found",request.Id);
            throw new ProjectTaskNotFoundException($"Project task with id {request.Id} was not found");
        }

        var projectTaskDto = projectTask.ToDto();
        _logger.LogInformation("Found project task {projectTaskDto}", JsonSerializer.Serialize(projectTaskDto, new JsonSerializerOptions { WriteIndented = true }));
        return projectTaskDto;
    }
}
