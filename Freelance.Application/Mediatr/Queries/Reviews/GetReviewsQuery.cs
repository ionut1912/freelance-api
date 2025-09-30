using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Reviews;

public record GetReviewsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<ReviewsDto>>;