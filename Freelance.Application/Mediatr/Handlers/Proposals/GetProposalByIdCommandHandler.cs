using Freelance.Application.Mediatr.Queries.Proposals;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Proposals;

public class GetProposalByIdCommandHandler : IRequestHandler<GetProposalByIdQuery, ProposalsDto>
{
    private readonly IProposalRepository _repository;

    public GetProposalByIdCommandHandler(IProposalRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        _repository = repository;
    }

    public async Task<ProposalsDto> Handle(GetProposalByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetProposalByIdAsync(request, cancellationToken);
    }
}