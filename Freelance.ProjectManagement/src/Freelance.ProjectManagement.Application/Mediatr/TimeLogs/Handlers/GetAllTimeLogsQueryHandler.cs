using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public sealed class GetAllTimeLogsQueryHandler(ITimeLogRepository timeLogRepository)
{
    public async Task<List<TimeLogDto>> Handle(GetAllTimeLogsQuery query, CancellationToken cancellationToken)
    {
        var timeLogs = await timeLogRepository.GetAllAsync(cancellationToken);
        return timeLogs.ToDtos();
    }
}
