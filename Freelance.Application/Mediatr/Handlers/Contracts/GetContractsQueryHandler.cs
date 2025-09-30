using Freelance.Application.Mediatr.Queries.Contracts;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Contracts;

public class GetContractsQueryHandler : IRequestHandler<GetContractsQuery, PaginatedList<ContractsDto>>
{
    private readonly IContractRepository _contractRepository;

    public GetContractsQueryHandler(IContractRepository contractRepository)
    {
        ArgumentNullException.ThrowIfNull(contractRepository, nameof(contractRepository));
        _contractRepository = contractRepository;
    }

    public async Task<PaginatedList<ContractsDto>> Handle(GetContractsQuery request,
        CancellationToken cancellationToken)
    {
        return await _contractRepository.GetContractsAsync(request, cancellationToken);
    }
}