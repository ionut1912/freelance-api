using Frelance.Application.Mediatr.Queries.Contracts;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Contracts;

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