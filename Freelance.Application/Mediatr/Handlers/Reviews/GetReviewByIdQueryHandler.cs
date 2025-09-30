using Freelance.Application.Mediatr.Queries.Reviews;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Reviews;

public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewsDto>
{
    private readonly IReviewRepository _repository;

    public GetReviewByIdQueryHandler(IReviewRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        _repository = repository;
    }

    public async Task<ReviewsDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetReviewsByIdAsync(request, cancellationToken);
    }
}