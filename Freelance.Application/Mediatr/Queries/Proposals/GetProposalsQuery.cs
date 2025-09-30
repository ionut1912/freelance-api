using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Proposals;

public record GetProposalsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<ProposalsDto>>;