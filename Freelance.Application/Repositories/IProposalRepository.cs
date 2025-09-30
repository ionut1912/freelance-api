using Freelance.Application.Mediatr.Commands.Proposals;
using Freelance.Application.Mediatr.Queries.Proposals;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface IProposalRepository
{
    Task CreateProposalAsync(CreateProposalCommand createProposalCommand, CancellationToken cancellationToken);

    Task<ProposalsDto> GetProposalByIdAsync(GetProposalByIdQuery getProposalByIdQuery,
        CancellationToken cancellationToken);

    Task<PaginatedList<ProposalsDto>> GetProposalsAsync(GetProposalsQuery getProposalsQuery,
        CancellationToken cancellationToken);

    Task UpdateProposalAsync(UpdateProposalCommand updateProposalCommand, CancellationToken cancellationToken);
    Task DeleteProposalAsync(DeleteProposalCommand deleteProposalCommand, CancellationToken cancellationToken);
}