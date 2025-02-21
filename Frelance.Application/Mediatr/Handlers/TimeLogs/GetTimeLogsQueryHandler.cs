using Frelance.Application.Mediatr.Queries.TimeLogs;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.TimeLogs;

public class GetTimeLogsQueryHandler : IRequestHandler<GetTimeLogsQuery, PaginatedList<TimeLogDto>>
{
    private readonly ITimeLogRepository _timeLogRepository;

    public GetTimeLogsQueryHandler(ITimeLogRepository timeLogRepository)
    {
        ArgumentNullException.ThrowIfNull(timeLogRepository, nameof(timeLogRepository));
        _timeLogRepository = timeLogRepository;
    }

    public async Task<PaginatedList<TimeLogDto>> Handle(GetTimeLogsQuery request, CancellationToken cancellationToken)
    {
        return await _timeLogRepository.GetTimeLogsAsync(request, cancellationToken);
    }
}