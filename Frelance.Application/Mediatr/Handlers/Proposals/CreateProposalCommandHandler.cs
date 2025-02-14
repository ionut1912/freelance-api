using Frelance.Application.Mediatr.Commands.Proposals;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Proposals;

public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, Unit>
{
    private readonly IProposalRepository _proposalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProposalCommandHandler(IProposalRepository proposalRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(proposalRepository, nameof(proposalRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _proposalRepository = proposalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        await _proposalRepository.AddProposalAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}