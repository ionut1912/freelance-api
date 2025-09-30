using Freelance.Application.Mediatr.Commands.Contracts;
using Freelance.Application.Mediatr.Queries.Contracts;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface IContractRepository
{
    Task CreateContractAsync(CreateContractCommand createContractCommand, CancellationToken cancellationToken);
    Task<ContractsDto> GetContractByIdAsync(GetContractByIdQuery query, CancellationToken cancellationToken);
    Task<PaginatedList<ContractsDto>> GetContractsAsync(GetContractsQuery query, CancellationToken cancellationToken);
    Task UpdateContractAsync(UpdateContractCommand updateContractCommand, CancellationToken cancellationToken);
    Task DeleteContractAsync(DeleteContractCommand deleteContractCommand, CancellationToken cancellationToken);
}