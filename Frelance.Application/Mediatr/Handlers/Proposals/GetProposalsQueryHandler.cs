using Frelance.Application.Mediatr.Queries.Proposals;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Proposals;

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