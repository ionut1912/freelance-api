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
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class TimeLogRepository : ITimeLogRepository
{
    private readonly IUserAccessor _userAccessor;
    private readonly IGenericRepository<ProjectTasks> _projectTasksRepository;
    private readonly IGenericRepository<FreelancerProfiles> _freelancerProfilesRepository;
    private readonly IGenericRepository<TimeLogs> _timeLogsRepository;

    public TimeLogRepository(IUserAccessor userAccessor,
        IGenericRepository<ProjectTasks> projectTasksRepository,
        IGenericRepository<FreelancerProfiles> freelancerProfilesRepository,
        IGenericRepository<TimeLogs> timeLogsRepository)
    {
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        ArgumentNullException.ThrowIfNull(projectTasksRepository, nameof(projectTasksRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfilesRepository, nameof(freelancerProfilesRepository));
        ArgumentNullException.ThrowIfNull(timeLogsRepository, nameof(timeLogsRepository));
        _userAccessor = userAccessor;
        _projectTasksRepository = projectTasksRepository;
        _freelancerProfilesRepository = freelancerProfilesRepository;
        _timeLogsRepository = timeLogsRepository;
    }
    public async Task AddTimeLogAsync(CreateTimeLogCommand createTimeLogCommand, CancellationToken cancellationToken)
    {
        var timeLogTask=await _projectTasksRepository.Query()
            .Where(x=>x.Title==createTimeLogCommand.CreateTimeLogRequest.TaskTitle)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Title)} : '{createTimeLogCommand.CreateTimeLogRequest.TaskTitle}' does not exist");
        }

        var timeLog = createTimeLogCommand.CreateTimeLogRequest.Adapt<TimeLogs>();
        timeLog.TaskId = timeLogTask.Id;
        timeLog.TotalHours = createTimeLogCommand.CreateTimeLogRequest.EndTime.Hour - createTimeLogCommand.CreateTimeLogRequest.StartTime.Hour;
        var freelancerProfile = _freelancerProfilesRepository.Query()
            .Where(x => x.Users.UserName == _userAccessor.GetUsername())
            .Include(x => x.Users)
            .FirstOrDefaultAsync(cancellationToken);
        timeLog.FreelancerProfileId = freelancerProfile.Id;
        await _timeLogsRepository.AddAsync(timeLog, cancellationToken);
    }

    public async Task UpdateTimeLogAsync(UpdateTimeLogCommand updateTimeLogCommand, CancellationToken cancellationToken)
    {
        if (updateTimeLogCommand.UpdateTimeLogRequest.TaskTitle is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Title)} : {updateTimeLogCommand.UpdateTimeLogRequest.TaskTitle} not found");
        }

        var timeLogTask = await _projectTasksRepository.Query()
            .Where(x => x.Title == updateTimeLogCommand.UpdateTimeLogRequest.TaskTitle)
            .FirstOrDefaultAsync(cancellationToken);
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Title)} : '{updateTimeLogCommand.UpdateTimeLogRequest.TaskTitle}' does not exist");
        }
        var timeLogToUpdate=await _timeLogsRepository.Query()
            .Where(x=>x.Id == updateTimeLogCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (timeLogToUpdate is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{updateTimeLogCommand.Id}' does not exist");
        }

        timeLogToUpdate = updateTimeLogCommand.UpdateTimeLogRequest.Adapt<TimeLogs>();
        timeLogToUpdate.TaskId = timeLogTask.Id;
        _timeLogsRepository.Update(timeLogToUpdate);
    }

    public async Task DeleteTimeLogAsync(DeleteTimeLogCommand deleteTimeLogCommand, CancellationToken cancellationToken)
    {
        var timeLogToRemove=await _timeLogsRepository.Query()
            .Where(x=>x.Id == deleteTimeLogCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (timeLogToRemove is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{deleteTimeLogCommand.Id}' does not exist");
        }
       _timeLogsRepository.Delete(timeLogToRemove);
    }

    public async Task<TimeLogDto> GetTimeLogByIdAsync(GetTimeLogByIdQuery getTimeLogByIdQuery, CancellationToken cancellationToken)
    {
        var timeLog=await _timeLogsRepository.Query()
            .Where(x=>x.Id == getTimeLogByIdQuery.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (timeLog is null)
        {
            throw new NotFoundException($"{nameof(TimeLogs)} with {nameof(TimeLogs.Id)} : '{getTimeLogByIdQuery.Id}' does not exist");
        }
        return timeLog.Adapt<TimeLogDto>();
    }

    public async Task<PaginatedList<TimeLogDto>> GetTimeLogsAsync(GetTimeLogsQuery getTimeLogsQuery, CancellationToken cancellationToken)
    {
        var timeLogsQuery=_timeLogsRepository.Query()
            .ProjectToType<TimeLogDto>();
        var count = await timeLogsQuery.CountAsync(cancellationToken);
        var items = await timeLogsQuery
            .Skip((getTimeLogsQuery.PaginationParams.PageNumber - 1) * getTimeLogsQuery.PaginationParams.PageSize)
            .Take(getTimeLogsQuery.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TimeLogDto>(items, count, getTimeLogsQuery.PaginationParams.PageNumber, getTimeLogsQuery.PaginationParams.PageSize);
    }
}