using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Projects;

public class DeleteProjectCommandHandler:IRequestHandler<DeleteProjectCommand,Unit>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        
    }
    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        await _projectRepository.DeleteProjectAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return  Unit.Value;
    }
}