using Freelance.Application.Mediatr.Commands.TimeLogs;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.TimeLogs;

public class UpdateTimeLogCommandHandler : IRequestHandler<UpdateTimeLogCommand>
{
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTimeLogCommandHandler(ITimeLogRepository timeLogRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(timeLogRepository, nameof(timeLogRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _timeLogRepository = timeLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTimeLogCommand request, CancellationToken cancellationToken)
    {
        await _timeLogRepository.UpdateTimeLogAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}