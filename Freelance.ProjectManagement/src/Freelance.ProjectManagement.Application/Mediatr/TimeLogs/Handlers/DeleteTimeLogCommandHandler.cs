using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public class DeleteTimeLogCommandHandler : IRequestHandler<DeleteTimeLogCommand, Unit>
{
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly ILogger<DeleteTimeLogCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTimeLogCommandHandler(ITimeLogRepository timeLogRepository, ILogger<DeleteTimeLogCommandHandler> logger,IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(timeLogRepository,nameof(timeLogRepository));
        ArgumentNullException.ThrowIfNull(logger,nameof(logger));
        ArgumentNullException.ThrowIfNull(unitOfWork,nameof(unitOfWork));
        _timeLogRepository = timeLogRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTimeLogCommand request, CancellationToken cancellationToken = default)
    {
        var timeLog = await _timeLogRepository.GetByIdAsync(request.Id, cancellationToken);
        if (timeLog == null)
        {
            _logger.LogError("TimeLog with Id {Id} was not found", request.Id);
            throw new TimeLogNotFoundException($"TimeLog with Id {request.Id} was not found");
        }
        _timeLogRepository.Delete(timeLog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Timelog with Id {Id} was deleted successfully", request.Id);
        return Unit.Value;
    }
}
