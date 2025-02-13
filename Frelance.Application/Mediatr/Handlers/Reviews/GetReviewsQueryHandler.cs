using Frelance.Application.Mediatr.Queries.Reviews;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Reviews;

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, PaginatedList<ReviewsDto>>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;

    }

    public async Task<PaginatedList<ReviewsDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return await _reviewRepository.GetReviewsAsync(request, cancellationToken);
    }
}