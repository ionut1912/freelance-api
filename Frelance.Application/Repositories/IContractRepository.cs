using Frelance.Application.Mediatr.Commands.Contracts;
using Frelance.Application.Mediatr.Queries.Contracts;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Contracts;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Application.Repositories;

public interface IContractRepository
{
    Task AddContractAsync(CreateContractCommand createContractCommand, CancellationToken cancellationToken);
    Task<ContractsDto> GetContractByIdAsync(GetContractByIdQuery query, CancellationToken cancellationToken);
    Task<PaginatedList<ContractsDto>> GetContractsAsync(GetContractsQuery query, CancellationToken cancellationToken);
    Task UpdateContractAsync(UpdateContractCommand updateContractCommand, CancellationToken cancellationToken);
    Task DeleteContractAsync(DeleteContractCommand deleteContractCommand, CancellationToken cancellationToken);
}