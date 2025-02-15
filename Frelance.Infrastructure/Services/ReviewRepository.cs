using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Reviews;
using Frelance.Application.Mediatr.Queries.Reviews;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;


public class ReviewRepository : IReviewRepository
{
    private readonly FrelanceDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public ReviewRepository(FrelanceDbContext context, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _context = context;
        _userAccessor = userAccessor;

    }

    public async Task AddReviewAsync(CreateReviewCommand createReviewCommand, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
        var review = createReviewCommand.Adapt<Reviews>();
        review.ReviewerId = user.Id;
        await _context.Reviews.AddAsync(review, cancellationToken);
    }

    public async Task<ReviewsDto> GetReviewsByIdAsync(GetReviewByIdQuery getReviewById, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
                                            .AsNoTracking()
                                            .Include(x => x.Reviewer)
                                            .FirstOrDefaultAsync(x => x.Id == getReviewById.Id, cancellationToken);
        if (review is null)
        {
            throw new NotFoundException($"{nameof(Reviews)} with {nameof(Reviews.Id)}: '{getReviewById.Id}' does not exist");
        }

        return review.Adapt<ReviewsDto>();
    }

    public async Task<PaginatedList<ReviewsDto>> GetReviewsAsync(GetReviewsQuery query, CancellationToken cancellationToken)
    {
        var reviewsQuery = _context.Reviews
            .AsNoTracking()
            .ProjectToType<ReviewsDto>();

        var count = await reviewsQuery.CountAsync(cancellationToken);
        var items = await reviewsQuery
            .Skip((query.PaginationParams.PageNumber - 1) * query.PaginationParams.PageSize)
            .Take(query.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReviewsDto>(items, count, query.PaginationParams.PageNumber, query.PaginationParams.PageSize);
    }

    public async Task UpdateReviewAsync(UpdateReviewCommand updateReviewCommand, CancellationToken cancellationToken)
    {
        var reviewToUpdate = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(x => x.Id == updateReviewCommand.Id, cancellationToken);
        if (reviewToUpdate is null)
        {
            throw new NotFoundException($"{nameof(Reviews)} with {nameof(Reviews.Id)}: '{updateReviewCommand.Id}' does not exist");
        }
        reviewToUpdate = updateReviewCommand.Adapt<Reviews>();
        _context.Reviews.Update(reviewToUpdate);
    }

    public async Task DeleteReviewAsync(DeleteReviewCommand deleteReviewCommand, CancellationToken cancellationToken)
    {
        var reviewToDelete = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(x => x.Id == deleteReviewCommand.Id, cancellationToken);
        if (reviewToDelete is null)
        {
            throw new NotFoundException($"{nameof(Reviews)} with {nameof(Reviews.Id)}: '{deleteReviewCommand.Id}' does not exist");
        }
        _context.Reviews.Remove(reviewToDelete);
    }
}