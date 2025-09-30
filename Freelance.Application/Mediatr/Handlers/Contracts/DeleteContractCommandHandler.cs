using Freelance.Application.Mediatr.Commands.Contracts;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Contracts;

public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand>
{
    private readonly IContractRepository _contractRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteContractCommandHandler(IContractRepository contractRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(contractRepository, nameof(contractRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _contractRepository = contractRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteContractCommand request, CancellationToken cancellationToken)
    {
        await _contractRepository.DeleteContractAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}