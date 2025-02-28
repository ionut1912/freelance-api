using Frelance.Application.Mediatr.Commands.Reviews;
using Frelance.Application.Mediatr.Queries.Reviews;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class ReviewRepository : IReviewRepository
{
    private readonly IGenericRepository<Reviews> _reviewRepository;
    private readonly IUserAccessor _userAccessor;
    private readonly IGenericRepository<Users> _userRepository;

    public ReviewRepository(IUserAccessor userAccessor
        , IGenericRepository<Users> userRepository
        , IGenericRepository<Reviews> reviewRepository)
    {
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(reviewRepository, nameof(reviewRepository));
        _userAccessor = userAccessor;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task CreateReviewAsync(CreateReviewCommand createReviewCommand, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Query()
            .Where(x => x.UserName == _userAccessor.GetUsername())
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            throw new NotFoundException(
                $"{nameof(Users)} with {nameof(Users.UserName)} {_userAccessor.GetUsername()} not found");

        var review = createReviewCommand.CreateReviewRequest.Adapt<Reviews>();
        review.ReviewerId = user.Id;
        await _reviewRepository.CreateAsync(review, cancellationToken);
    }

    public async Task<ReviewsDto> GetReviewsByIdAsync(GetReviewByIdQuery getReviewById,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.Query()
            .Where(x => x.Id == getReviewById.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (review is null)
            throw new NotFoundException(
                $"{nameof(Reviews)} with {nameof(Reviews.Id)}: '{getReviewById.Id}' does not exist");

        return review.Adapt<ReviewsDto>();
    }

    public async Task<PaginatedList<ReviewsDto>> GetReviewsAsync(GetReviewsQuery query,
        CancellationToken cancellationToken)
    {
        var reviewsQuery = _reviewRepository.Query()
            .ProjectToType<ReviewsDto>();

        var count = await reviewsQuery.CountAsync(cancellationToken);
        var items = await reviewsQuery
            .Skip((query.PaginationParams.PageNumber - 1) * query.PaginationParams.PageSize)
            .Take(query.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReviewsDto>(items, count, query.PaginationParams.PageNumber,
            query.PaginationParams.PageSize);
    }

    public async Task UpdateReviewAsync(UpdateReviewCommand updateReviewCommand, CancellationToken cancellationToken)
    {
        var reviewToUpdate = await _reviewRepository.Query()
            .Where(x => x.Id == updateReviewCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (reviewToUpdate is null)
            throw new NotFoundException(
                $"{nameof(Reviews)} with {nameof(Reviews.Id)}: '{updateReviewCommand.Id}' does not exist");
        reviewToUpdate = updateReviewCommand.UpdateReviewRequest.Adapt<Reviews>();
        _reviewRepository.Update(reviewToUpdate);
    }

    public async Task DeleteReviewAsync(DeleteReviewCommand deleteReviewCommand, CancellationToken cancellationToken)
    {
        var reviewToDelete = await _reviewRepository.Query()
            .Where(x => x.Id == deleteReviewCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (reviewToDelete is null)
            throw new NotFoundException(
                $"{nameof(Reviews)} with {nameof(Reviews.Id)}: '{deleteReviewCommand.Id}' does not exist");
        _reviewRepository.Delete(reviewToDelete);
    }
}