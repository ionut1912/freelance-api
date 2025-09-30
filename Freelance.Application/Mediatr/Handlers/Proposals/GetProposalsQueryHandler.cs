using Freelance.Application.Mediatr.Queries.Proposals;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Proposals;

public class GetProposalsQueryHandler : IRequestHandler<GetProposalsQuery, PaginatedList<ProposalsDto>>
{
    private readonly IProposalRepository _proposalRepository;

    public GetProposalsQueryHandler(IProposalRepository proposalRepository)
    {
        ArgumentNullException.ThrowIfNull(proposalRepository, nameof(proposalRepository));
        _proposalRepository = proposalRepository;
    }

    public async Task<PaginatedList<ProposalsDto>> Handle(GetProposalsQuery request,
        CancellationToken cancellationToken)
    {
        return await _proposalRepository.GetProposalsAsync(request, cancellationToken);
    }
}