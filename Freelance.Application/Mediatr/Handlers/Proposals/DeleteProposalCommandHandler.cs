using Freelance.Application.Mediatr.Commands.Proposals;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Proposals;

public class DeleteProposalCommandHandler : IRequestHandler<DeleteProposalCommand>
{
    private readonly IProposalRepository _proposalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProposalCommandHandler(IProposalRepository proposalRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(proposalRepository, nameof(proposalRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _proposalRepository = proposalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProposalCommand request, CancellationToken cancellationToken)
    {
        await _proposalRepository.DeleteProposalAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}