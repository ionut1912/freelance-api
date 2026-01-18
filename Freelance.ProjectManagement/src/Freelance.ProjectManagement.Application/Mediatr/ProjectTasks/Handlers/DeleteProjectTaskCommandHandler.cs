using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;

public class DeleteProjectTaskCommandHandler : IRequestHandler<DeleteProjectTaskCommand, Unit>
{

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProjectTaskCommandHandler> _logger;

    public DeleteProjectTaskCommandHandler(IProjectTaskRepository projectTaskRepository,IUnitOfWork unitOfWork,ILogger<DeleteProjectTaskCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectTaskRepository, nameof(projectTaskRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectTaskRepository = projectTaskRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken = default)
    {
        var projectTask = await _projectTaskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (projectTask == null)
        {
            _logger.LogError("We can't delete project task,because project Task with Id {Id} was not found", request.Id);
            throw new ProjectTaskNotFoundException($"Project task with id {request.Id} was not found");
        }

        _projectTaskRepository.Delete(projectTask);
        _logger.LogInformation("Project Task with Id {Id} deleted", request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
