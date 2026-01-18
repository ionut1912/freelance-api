using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public class UpdateProjectTaskCommandHandler : IRequestHandler<UpdateProjectTaskCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProjectTaskCommandHandler> _logger;

    public UpdateProjectTaskCommandHandler(IProjectRepository projectRepository,IProjectTaskRepository projectTaskRepository,IUnitOfWork unitOfWork,ILogger<UpdateProjectTaskCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(projectTaskRepository,nameof(projectTaskRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectRepository = projectRepository;
        _projectTaskRepository = projectTaskRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var status = ProjectTaskStatus.New;
        var priority = ProjectTaskPriority.Low;
        var project = await _projectRepository.GetByIdWithTrackingAsync(request.ProjectId, cancellationToken,p=>p.Tasks);
        var projectTask = await _projectTaskRepository.GetByIdWithTrackingAsync(request.Id, cancellationToken);
        if (project == null)
        {
            _logger.LogInformation("We can't update projectTask,because project with Id {ProjectId} was not found", request.ProjectId);
            throw new ProjectNotFoundException($"Project with Id {request.ProjectId} was not found");
        }
        if (projectTask == null)
        {
            _logger.LogInformation("We can't update projectTask,because project task with Id {Id} was not found", request.Id);
            throw new ProjectTaskNotFoundException($"Project task with Id {request.Id} was not found");
        }

        status = request.Status switch
        {
            "New" => ProjectTaskStatus.New,
            "InProgress" => ProjectTaskStatus.InProgress,
            "Review" => ProjectTaskStatus.Review,
            "Done" => ProjectTaskStatus.Done,
            _ => throw new NotImplementedException()
        };

        priority = request.Priority switch
        {
            "Low" => ProjectTaskPriority.Low,
            "Medium" => ProjectTaskPriority.Medium,
            "High" => ProjectTaskPriority.High,
            _ => throw new NotImplementedException()
        };
        projectTask.Update(request.Title, request.Description, status, priority);
        project.UpdateTask(projectTask);
        _projectTaskRepository.Update(projectTask);
        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Project task with Id {Id} was updated successfully", request.Id);
        return Unit.Value;
    }
}
