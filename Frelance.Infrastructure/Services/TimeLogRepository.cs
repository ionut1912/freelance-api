using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Queries.TimeLogs;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Responses.TimeLogs;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class TimeLogRepository : ITimeLogRepository
{
    private readonly FrelanceDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public TimeLogRepository(FrelanceDbContext dbContext, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _context = dbContext;
        _userAccessor = userAccessor;
    }
    public async Task AddTimeLogAsync(CreateTimeLogCommand createTimeLogCommand, CancellationToken cancellationToken)
    {
        var timeLogTask = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Title == createTimeLogCommand.TaskTitle, cancellationToken);
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Title)} : '{createTimeLogCommand.TaskTitle}' does not exist");
        }

        var timeLog = createTimeLogCommand.Adapt<TimeLogs>();
        timeLog.TaskId = timeLogTask.Id;
        timeLog.TotalHours = createTimeLogCommand.EndTime.Hour - createTimeLogCommand.StartTime.Hour;
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);
        timeLog.User = user;
        await _context.TimeLogs.AddAsync(timeLog, cancellationToken);
    }

    public async Task UpdateTimeLogAsync(UpdateTimeLogCommand updateTimeLogCommand, CancellationToken cancellationToken)
    {
        var timeLogTask = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Title == updateTimeLogCommand.TaskTitle, cancellationToken);
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Title)} : '{updateTimeLogCommand.TaskTitle}' does not exist");
        }
        var timeLogToUpdate = await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == updateTimeLogCommand.Id, cancellationToken);
        if (timeLogToUpdate is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{updateTimeLogCommand.Id}' does not exist");
        }

        timeLogToUpdate.TaskId = timeLogTask.Id;
        timeLogToUpdate.StartTime = updateTimeLogCommand.StartTime;
        if (timeLogToUpdate.Date == updateTimeLogCommand.Date)
        {
            timeLogToUpdate.TotalHours = timeLogToUpdate.TotalHours + updateTimeLogCommand.TotalHours;
        }
        timeLogToUpdate.TotalHours = updateTimeLogCommand.EndTime.Hour - updateTimeLogCommand.StartTime.Hour;
        _context.TimeLogs.Update(timeLogToUpdate);
    }

    public async Task DeleteTimeLogAsync(DeleteTimeLogCommand deleteTimeLogCommand, CancellationToken cancellationToken)
    {
        var timeLogToRemove = await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == deleteTimeLogCommand.Id, cancellationToken);
        if (timeLogToRemove is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{deleteTimeLogCommand.Id}' does not exist");
        }
        _context.TimeLogs.Remove(timeLogToRemove);
    }

    public async Task<GetTimeLogByIdResponse> GetTimeLogByIdAsync(GetTimeLogByIdQuery getTimeLogByIdQuery, CancellationToken cancellationToken)
    {
        var timeLog = await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == getTimeLogByIdQuery.Id, cancellationToken);
        if (timeLog is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{getTimeLogByIdQuery.Id}' does not exist");
        }
        return timeLog.Adapt<GetTimeLogByIdResponse>();
    }

    public async Task<PaginatedList<TimeLogDto>> GetTimeLogsAsync(GetTimeLogsQuery getTimeLogsQuery, CancellationToken cancellationToken)
    {
        var timeLogsQuery = _context.TimeLogs.ProjectToType<TimeLogDto>().AsQueryable();
        return await CollectionHelper<TimeLogDto>.ToPaginatedList(timeLogsQuery, getTimeLogsQuery.PaginationParams.PageNumber, getTimeLogsQuery.PaginationParams.PageSize);
    }
}