using Frelance.Application.Mediatr.Queries.Contracts;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Contracts;

public class GetContractByIdQueryHandler:IRequestHandler<GetContractByIdQuery, ContractsDto>
{
    private readonly IContractRepository _contractRepository;

    public GetContractByIdQueryHandler(IContractRepository contractRepository)
    {
        ArgumentNullException.ThrowIfNull(contractRepository, nameof(contractRepository));
        _contractRepository = contractRepository;
    }
    
    public async Task<ContractsDto> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
    {
        return await _contractRepository.GetContractByIdAsync(request,cancellationToken);
    }
}