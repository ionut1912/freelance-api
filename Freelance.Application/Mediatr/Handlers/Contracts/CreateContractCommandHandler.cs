using Freelance.Application.Mediatr.Commands.Contracts;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Contracts;

public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand>
{
    private readonly IContractRepository _contractRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContractCommandHandler(IContractRepository contractRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(contractRepository, nameof(contractRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _contractRepository = contractRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        await _contractRepository.CreateContractAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}