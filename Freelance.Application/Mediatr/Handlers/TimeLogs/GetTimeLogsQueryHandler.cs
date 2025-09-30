using Freelance.Application.Mediatr.Queries.TimeLogs;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.TimeLogs;

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