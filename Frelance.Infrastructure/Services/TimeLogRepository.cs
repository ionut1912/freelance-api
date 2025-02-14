using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Queries.TimeLogs;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
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
        var timeLogTask = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Title == createTimeLogCommand.CreateTimeLogRequest.TaskTitle, cancellationToken);
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Title)} : '{createTimeLogCommand.CreateTimeLogRequest.TaskTitle}' does not exist");
        }

        var timeLog = createTimeLogCommand.Adapt<TimeLogs>();
        timeLog.TaskId = timeLogTask.Id;
        timeLog.TotalHours = createTimeLogCommand.CreateTimeLogRequest.EndTime.Hour - createTimeLogCommand.CreateTimeLogRequest.StartTime.Hour;
        var freelancerProfile = await _context.FreelancerProfiles
                                      .AsNoTracking()
                                      .Include(x => x.Users)
                                      .FirstOrDefaultAsync(x => x.Users.UserName == _userAccessor.GetUsername(), cancellationToken);

        timeLog.FreelancerProfileId = freelancerProfile.Id;
        timeLog.CreatedAt = DateTime.UtcNow;
        await _context.TimeLogs.AddAsync(timeLog, cancellationToken);
    }

    public async Task UpdateTimeLogAsync(UpdateTimeLogCommand updateTimeLogCommand, CancellationToken cancellationToken)
    {
        var timeLogTask = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Title == updateTimeLogCommand.UpdateTimeLogRequest.TaskTitle, cancellationToken);
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Title)} : '{updateTimeLogCommand.UpdateTimeLogRequest.TaskTitle}' does not exist");
        }
        var timeLogToUpdate = await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == updateTimeLogCommand.Id, cancellationToken);
        if (timeLogToUpdate is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{updateTimeLogCommand.Id}' does not exist");
        }

        timeLogToUpdate.TaskId = timeLogTask.Id;
        timeLogToUpdate.StartTime = updateTimeLogCommand.UpdateTimeLogRequest.StartTime;
        timeLogToUpdate.UpdatedAt = DateTime.UtcNow;
        timeLogToUpdate.TotalHours = updateTimeLogCommand.UpdateTimeLogRequest.EndTime.Hour - updateTimeLogCommand.UpdateTimeLogRequest.StartTime.Hour;
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

    public async Task<TimeLogDto> GetTimeLogByIdAsync(GetTimeLogByIdQuery getTimeLogByIdQuery, CancellationToken cancellationToken)
    {
        var timeLog = await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == getTimeLogByIdQuery.Id, cancellationToken);
        if (timeLog is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{getTimeLogByIdQuery.Id}' does not exist");
        }
        return timeLog.Adapt<TimeLogDto>();
    }

    public async Task<PaginatedList<TimeLogDto>> GetTimeLogsAsync(GetTimeLogsQuery getTimeLogsQuery, CancellationToken cancellationToken)
    {
        var timeLogsQuery = _context.TimeLogs
            .AsNoTracking()
            .ProjectToType<TimeLogDto>();

        var count = await timeLogsQuery.CountAsync(cancellationToken);
        var items = await timeLogsQuery
            .Skip((getTimeLogsQuery.PaginationParams.PageNumber - 1) * getTimeLogsQuery.PaginationParams.PageSize)
            .Take(getTimeLogsQuery.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TimeLogDto>(items, count, getTimeLogsQuery.PaginationParams.PageNumber, getTimeLogsQuery.PaginationParams.PageSize);
    }
}