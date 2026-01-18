using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
{

    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProjectCommandHandler> _logger;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository,IUnitOfWork unitOfWork,ILogger<DeleteProjectCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken, p => p.Tasks, p => p.Technologies);

        if (project == null)
        {
            _logger.LogError("We can't delete project,because project with Id {Id} was not found", request.Id);
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");
        }

        _projectRepository.Delete(project);
        _logger.LogInformation("Project with Id {Id} was deleted", request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
