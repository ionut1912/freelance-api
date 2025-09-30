using Freelance.Application.Mediatr.Commands.Proposals;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Proposals;

public class UpdateProposalCommandHandler : IRequestHandler<UpdateProposalCommand>
{
    private readonly IProposalRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProposalCommandHandler(IProposalRepository repository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateProposalCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdateProposalAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}