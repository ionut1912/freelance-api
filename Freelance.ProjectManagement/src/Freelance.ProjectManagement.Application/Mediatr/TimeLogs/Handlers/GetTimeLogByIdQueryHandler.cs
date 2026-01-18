using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public class GetTimeLogByIdQueryHandler : IRequestHandler<GetTimeLogByIdQuery, TimeLogDto>
{
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly ILogger<GetTimeLogByIdQueryHandler> _logger;

    public GetTimeLogByIdQueryHandler(ITimeLogRepository timeLogRepository,ILogger<GetTimeLogByIdQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(timeLogRepository, nameof(timeLogRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _timeLogRepository = timeLogRepository;
        _logger = logger;
    }

    public  async Task<TimeLogDto> Handle(GetTimeLogByIdQuery request, CancellationToken cancellationToken = default)
    {
        var timeLog = await _timeLogRepository.GetByIdAsync(request.Id, cancellationToken);
        if (timeLog == null)
        {
            _logger.LogError("TimeLog with Id {Id} was not found", request.Id);
            throw new TimeLogNotFoundException($"TimeLog with Id {request.Id} was not found");
        }
        var timeLogDto = timeLog.ToDto();
        _logger.LogInformation("Found timeLog {timeLogDto}", JsonSerializer.Serialize(timeLogDto, new JsonSerializerOptions { WriteIndented = true }));
        return timeLogDto;
    }
}
