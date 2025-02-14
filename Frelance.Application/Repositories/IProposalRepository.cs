using Frelance.Application.Mediatr.Commands.Proposals;
using Frelance.Application.Mediatr.Queries.Proposals;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Application.Repositories;

public interface IProposalRepository
{
    Task AddProposalAsync(CreateProposalCommand createProposalCommand, CancellationToken cancellationToken);
    Task<ProposalsDto> GetProposalByIdAsync(GetProposalByIdQuery getProposalByIdQuery, CancellationToken cancellationToken);
    Task<PaginatedList<ProposalsDto>> GetProposalsAsync(GetProposalsQuery getProposalsQuery, CancellationToken cancellationToken);
    Task UpdateProposalAsync(UpdateProposalCommand updateProposalCommand, CancellationToken cancellationToken);
    Task DeleteProposalAsync(DeleteProposalCommand deleteProposalCommand, CancellationToken cancellationToken);
}