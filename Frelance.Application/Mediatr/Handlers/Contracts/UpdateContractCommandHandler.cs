using Frelance.Application.Mediatr.Commands.Contracts;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Contracts;

public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, Unit>
{
    private readonly IContractRepository _contractRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateContractCommandHandler(IContractRepository contractRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(contractRepository, nameof(contractRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _contractRepository = contractRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Unit> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
    {
        await _contractRepository.UpdateContractAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}