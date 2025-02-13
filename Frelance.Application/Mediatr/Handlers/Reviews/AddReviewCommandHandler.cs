using Frelance.Application.Mediatr.Commands.Reviews;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Reviews;

public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Unit>
{
    private readonly IReviewRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddReviewCommandHandler(IReviewRepository repository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        await _repository.AddReviewAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}