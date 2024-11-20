using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.TimeLogs;

public class DeleteTimeLogCommandHandler:IRequestHandler<DeleteTimeLogCommand,Unit>
{
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTimeLogCommandHandler(ITimeLogRepository timeLogRepository, IUnitOfWork unitOfWork)
    {
        _timeLogRepository = timeLogRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(DeleteTimeLogCommand request, CancellationToken cancellationToken)
    {

        await _timeLogRepository.DeleteTimeLogAsync(request,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}