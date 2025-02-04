using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.TimeLogs;

public class UpdateTimeLogCommandHandler:IRequestHandler<UpdateTimeLogCommand,Unit>
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
    public  async Task<Unit> Handle(UpdateTimeLogCommand request, CancellationToken cancellationToken)
    {
        await _timeLogRepository.UpdateTimeLogAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}