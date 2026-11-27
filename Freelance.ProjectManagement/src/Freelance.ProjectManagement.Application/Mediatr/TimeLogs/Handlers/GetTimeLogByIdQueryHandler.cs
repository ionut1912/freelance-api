using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;

public sealed class GetTimeLogByIdQueryHandler(ITimeLogRepository timeLogRepository)
{
    public async Task<TimeLogDto> Handle(GetTimeLogByIdQuery query, CancellationToken cancellationToken)
    {
        var timeLog = await timeLogRepository.GetByIdAsync(query.Id, cancellationToken)
                      ?? throw new TimeLogNotFoundException($"TimeLog with Id {query.Id} was not found");

        return timeLog.ToDto();
    }
}
