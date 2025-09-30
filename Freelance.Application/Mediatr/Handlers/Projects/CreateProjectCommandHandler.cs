using Freelance.Application.Mediatr.Commands.Projects;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Projects;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        await _projectRepository.CreateProjectAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}