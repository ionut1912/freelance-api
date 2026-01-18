using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public class UpdateTimeLogCommandHandler : IRequestHandler<UpdateTimeLogCommand, Unit>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly ILogger<UpdateTimeLogCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTimeLogCommandHandler(IProjectTaskRepository projectTaskRepository,ITimeLogRepository timeLogRepository,ILogger<UpdateTimeLogCommandHandler> logger,IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(projectTaskRepository, nameof(projectTaskRepository));
        ArgumentNullException.ThrowIfNull(timeLogRepository,nameof(timeLogRepository));
        ArgumentNullException.ThrowIfNull(logger,nameof(logger));
        ArgumentNullException.ThrowIfNull(unitOfWork,nameof(unitOfWork));
        _projectTaskRepository = projectTaskRepository;
        _timeLogRepository = timeLogRepository;
        _logger = logger;
        _unitOfWork= unitOfWork;
    }

    public async Task<Unit> Handle(UpdateTimeLogCommand request, CancellationToken cancellationToken = default)
    {
        var projectTask = await _projectTaskRepository.GetByIdWithTrackingAsync(request.TaskId, cancellationToken, pt => pt.TimeLogs);
        var timeLog = await _timeLogRepository.GetByIdWithTrackingAsync(request.Id, cancellationToken);
        if (projectTask == null)
        {
            _logger.LogError("We can not update timeLog,because Project Task with Id {TaskId} was not found", request.TaskId);
            throw new ProjectTaskNotFoundException($"Project task with id {request.TaskId} was not found");
        }
        if (timeLog == null) 
        {
            _logger.LogError("We can not update timeLog,because TimeLog With Id {Id} was not found",request.Id);
            throw new TimeLogNotFoundException($"Time Log with Id {request.Id} was not found");
        }
        timeLog.Update(request.StartTime, request.EndTime);
        projectTask.UpdateTimelog(timeLog);
        _timeLogRepository.Update(timeLog);
        _projectTaskRepository.Update(projectTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Time Log with Id {Id} updated successfully", request.Id);
        return Unit.Value;
    }
}
