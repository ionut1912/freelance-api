using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.Application.Mappings;

public static class TimeLogMapping
{
    extension(TimeLog timeLog)
    {
        public TimeLogDto ToDto() =>
            new(timeLog.Id, timeLog.TaskId, timeLog.StartTime, timeLog.EndTime, timeLog.TotalHours);
    }

    extension(IEnumerable<TimeLog> timeLogs)
    {
        public List<TimeLogDto> ToDtos() => [.. timeLogs.Select(tl => tl.ToDto())];
    }

    extension(CreateTimeLogRequest request)
    {
        public CreateTimeLogCommand ToCreateCommand() => new(request.TaskId, request.StartTime, request.EndTime);
    }

    extension(UpdateTimeLogRequest request)
    {
        public UpdateTimeLogCommand ToUpdateCommand(Guid id) => new(id, request.TaskId, request.StartTime, request.EndTime);
    }
}
