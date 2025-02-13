using Frelance.Application.Mediatr.Commands.Reviews;
using Frelance.Application.Mediatr.Queries.Reviews;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Application.Repositories;

public interface IReviewRepository
{
    Task AddReviewAsync(AddReviewCommand addReviewCommand, CancellationToken cancellationToken );
    Task<ReviewsDto> GetReviewsByIdAsync(GetReviewByIdQuery id, CancellationToken cancellationToken);
    Task<PaginatedList<ReviewsDto>> GetReviewsAsync(GetReviewsQuery query, CancellationToken cancellationToken);
    Task UpdateReviewAsync(UpdateReviewCommand updateReviewCommand, CancellationToken cancellationToken);
    Task DeleteReviewAsync(DeleteReviewCommand deleteReviewCommand, CancellationToken cancellationToken);
}