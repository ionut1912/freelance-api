using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Contracts;

public record GetContractsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<ContractsDto>>;