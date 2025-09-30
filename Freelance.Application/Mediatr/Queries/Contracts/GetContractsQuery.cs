using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Contracts;

public record GetContractsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<ContractsDto>>;