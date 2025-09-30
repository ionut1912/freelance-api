using Freelance.Application.Mediatr.Commands.TimeLogs;
using Freelance.Application.Mediatr.Queries.TimeLogs;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface ITimeLogRepository
{
    Task CreateTimeLogAsync(CreateTimeLogCommand createTimeLogCommand, CancellationToken cancellationToken);
    Task UpdateTimeLogAsync(UpdateTimeLogCommand updateTimeLogCommand, CancellationToken cancellationToken);
    Task DeleteTimeLogAsync(DeleteTimeLogCommand deleteTimeLogCommand, CancellationToken cancellationToken);
    Task<TimeLogDto> GetTimeLogByIdAsync(GetTimeLogByIdQuery getTimeLogByIdQuery, CancellationToken cancellationToken);

    Task<PaginatedList<TimeLogDto>> GetTimeLogsAsync(GetTimeLogsQuery getTimeLogsQuery,
        CancellationToken cancellationToken);
}