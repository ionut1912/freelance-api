using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Reviews;

public record GetReviewsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<ReviewsDto>>;
