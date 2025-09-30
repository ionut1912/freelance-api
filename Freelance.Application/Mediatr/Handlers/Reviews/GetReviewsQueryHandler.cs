using Freelance.Application.Mediatr.Queries.Reviews;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Reviews;

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