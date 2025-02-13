using Frelance.Application.Mediatr.Queries.Reviews;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Reviews;

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