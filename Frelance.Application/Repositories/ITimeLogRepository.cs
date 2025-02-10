using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Queries.TimeLogs;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Responses.TimeLogs;

namespace Frelance.Application.Repositories;

public interface ITimeLogRepository
{
    Task AddTimeLogAsync(CreateTimeLogCommand createTimeLogCommand, CancellationToken cancellationToken);
    Task UpdateTimeLogAsync(UpdateTimeLogCommand updateTimeLogCommand, CancellationToken cancellationToken);
    Task DeleteTimeLogAsync(DeleteTimeLogCommand deleteTimeLogCommand, CancellationToken cancellationToken);
    Task<GetTimeLogByIdResponse> GetTimeLogByIdAsync(GetTimeLogByIdQuery getTimeLogByIdQuery, CancellationToken cancellationToken);
    Task<PaginatedList<TimeLogDto>> GetTimeLogsAsync(GetTimeLogsQuery getTimeLogsQuery, CancellationToken cancellationToken);
}