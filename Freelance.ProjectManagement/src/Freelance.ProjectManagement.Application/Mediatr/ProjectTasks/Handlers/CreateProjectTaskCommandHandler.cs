using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
using System.Data;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public class CreateProjectTaskCommandHandler : IRequestHandler<CreateProjectTaskCommand, ProjectTask>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProjectTaskCommandHandler> _logger;

    public CreateProjectTaskCommandHandler(IProjectRepository projectRepository,IProjectTaskRepository projectTaskRepository,IUnitOfWork unitOfWork,ILogger<CreateProjectTaskCommandHandler>logger)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(projectTaskRepository, nameof(projectTaskRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork,nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger,nameof(logger));
        _projectRepository = projectRepository;
        _projectTaskRepository = projectTaskRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ProjectTask> Handle(CreateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var status = ProjectTaskStatus.New;
        var priority = ProjectTaskPriority.Low;
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken,p=>p.Tasks);
        if(project == null)
        {
            _logger.LogInformation("We can't create task,because project with Id {ProjectId} was not found", request.ProjectId);
            throw new ProjectNotFoundException($"Project with Id {request.ProjectId} was not found");
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
            "Medium"=> ProjectTaskPriority.Medium,
            "High"=>ProjectTaskPriority.High,
            _=> throw new NotImplementedException()
        };

        var task = ProjectTask.Create(project.Id, request.Title, request.Description, status,priority);
        await _projectTaskRepository.AddAsync(task,cancellationToken);
        project.AddTask(task);
        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Task with Id {Id} was created", task.Id);
        return task;
    }
}
