using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.Application.Mappings;

public static class TimeLogMapping
{
    public static TimeLogDto ToDto(this TimeLog timeLog)
    {
        return new TimeLogDto(
            timeLog.Id,
            timeLog.TaskId,
            timeLog.StartTime,
            timeLog.EndTime,
            timeLog.TotalHours);
    }

    public static List<TimeLogDto> ToDtos(this IEnumerable<TimeLog> timeLogs)
    {
        return [.. timeLogs.Select(tl => tl.ToDto())];

    }

    public static CreateTimeLogCommand ToCreateCommand(this CreateTimeLogRequest request)
    {
        return new CreateTimeLogCommand(request.TaskId, request.StartTime, request.EndTime);
    }

    public static UpdateTimeLogCommand ToUpdateCommand(this UpdateTimeLogRequest request,Guid Id)
    {
        return new UpdateTimeLogCommand(Id,request.TaskId,request.StartTime, request.EndTime);
    }
}
