using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Contracts;

public record GetContractByIdQuery(int Id) : IRequest<ContractsDto>;