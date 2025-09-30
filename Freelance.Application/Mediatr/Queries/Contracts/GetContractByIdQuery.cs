using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Contracts;

public record GetContractByIdQuery(int Id) : IRequest<ContractsDto>;