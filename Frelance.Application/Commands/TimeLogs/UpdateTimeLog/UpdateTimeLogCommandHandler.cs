using Frelance.Contracts.Exceptions;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Commands.TimeLogs.UpdateTimeLog;

public class UpdateTimeLogCommandHandler:IRequestHandler<UpdateTimeLogCommand,Unit>
{
    private readonly FrelanceDbContext _context;

    public UpdateTimeLogCommandHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public  async Task<Unit> Handle(UpdateTimeLogCommand request, CancellationToken cancellationToken)
    {
        var timeLogTask=await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x=>x.Title==request.TaskTitle, cancellationToken);
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTask)} with {nameof(ProjectTask.Title)} : '{request.TaskTitle}' does not exist");
        }
        var timeLogToUpdate=await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==request.Id, cancellationToken);
        if (timeLogToUpdate is null)
        {
            throw new NotFoundException($"{nameof(TimeLog)} with {nameof(TimeLog.Id)} : '{request.Id}' does not exist");
        }

        timeLogToUpdate.TaskId = timeLogTask.Id;
        timeLogToUpdate.StartTime = request.StartTime;
        if (timeLogToUpdate.Date == request.Date)
        {
            timeLogToUpdate.TotalHours=timeLogToUpdate.TotalHours+request.TotalHours;
        }
        timeLogToUpdate.TotalHours=request.EndTime.Hour-request.StartTime.Hour;
        _context.TimeLogs.Update(timeLogToUpdate);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}