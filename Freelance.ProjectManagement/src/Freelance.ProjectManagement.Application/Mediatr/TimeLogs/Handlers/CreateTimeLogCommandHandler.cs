using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public class CreateTimeLogCommandHandler : IRequestHandler<CreateTimeLogCommand, TimeLog>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly ILogger<CreateTimeLogCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTimeLogCommandHandler(IProjectTaskRepository projectTaskRepository,ITimeLogRepository timeLogRepository,ILogger<CreateTimeLogCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(projectTaskRepository, nameof(projectTaskRepository));
        ArgumentNullException.ThrowIfNull(timeLogRepository, nameof(timeLogRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _projectTaskRepository = projectTaskRepository;
        _timeLogRepository = timeLogRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<TimeLog> Handle(CreateTimeLogCommand request, CancellationToken cancellationToken)
    {
        var projectTask = await _projectTaskRepository.GetByIdAsync(request.TaskId, cancellationToken, pt => pt.TimeLogs);
        if (projectTask == null)
        {
            _logger.LogError("We can not create timeLog,because Project Task with Id {TaskId} was not found", request.TaskId);
            throw new ProjectTaskNotFoundException($"Project task with id {request.TaskId} was not found");
        }

        var timeLog = TimeLog.Create(request.TaskId, request.StartTime, request.EndTime);
        await _timeLogRepository.AddAsync(timeLog, cancellationToken);
        projectTask.AddTimeLog(timeLog);
        _projectTaskRepository.Update(projectTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("TimeLog wiht Id {Id} was created", timeLog.Id);
        return timeLog;
    }
}
