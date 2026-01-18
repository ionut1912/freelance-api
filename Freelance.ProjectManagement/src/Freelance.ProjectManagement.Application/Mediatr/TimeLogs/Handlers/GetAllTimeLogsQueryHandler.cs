using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public class GetAllTimeLogsQueryHandler : IRequestHandler<GetAllTimeLogsQuery, List<TimeLogDto>>
{
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly ILogger<GetAllTimeLogsQueryHandler> _logger;

    public GetAllTimeLogsQueryHandler(ITimeLogRepository timeLogRepository,ILogger<GetAllTimeLogsQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(timeLogRepository, nameof(timeLogRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _timeLogRepository = timeLogRepository;
        _logger = logger;
    }

    public async Task<List<TimeLogDto>> Handle(GetAllTimeLogsQuery request, CancellationToken cancellationToken = default)
    {
        var timeLogs = await _timeLogRepository.GetAllAsync(cancellationToken);
        var timeLogDtos = timeLogs.ToDtos();
        _logger.LogInformation("Found timeLogs {timeLogDtos}", JsonSerializer.Serialize(timeLogDtos, new JsonSerializerOptions { WriteIndented = true }));
        return timeLogDtos;
    }
}
