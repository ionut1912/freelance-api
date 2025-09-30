using Freelance.Application.Mediatr.Commands.Reviews;
using Freelance.Application.Mediatr.Queries.Reviews;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface IReviewRepository
{
    Task CreateReviewAsync(CreateReviewCommand createReviewCommand, CancellationToken cancellationToken);
    Task<ReviewsDto> GetReviewsByIdAsync(GetReviewByIdQuery id, CancellationToken cancellationToken);
    Task<PaginatedList<ReviewsDto>> GetReviewsAsync(GetReviewsQuery query, CancellationToken cancellationToken);
    Task UpdateReviewAsync(UpdateReviewCommand updateReviewCommand, CancellationToken cancellationToken);
    Task DeleteReviewAsync(DeleteReviewCommand deleteReviewCommand, CancellationToken cancellationToken);
}