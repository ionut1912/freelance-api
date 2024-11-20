using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Contracts.Exceptions;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Mediatr.Handlers.TimeLogs;

public class CreateTimeLogCommandHandler:IRequestHandler<CreateTimeLogCommand,int>
{
    private readonly FrelanceDbContext _context;

    public CreateTimeLogCommandHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateTimeLogCommand request, CancellationToken cancellationToken)
    {
        var timeLogTask=await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x=>x.Title==request.TaskTitle, cancellationToken);
        if (timeLogTask is null)
        {
            throw new NotFoundException($"{nameof(ProjectTask)} with {nameof(ProjectTask.Title)} : '{request.TaskTitle}' does not exist");
        }

        var timeLog = new TimeLog
        {
            TaskId = timeLogTask.Id,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Date = request.Date,
            TotalHours = request.EndTime.Hour - request.StartTime.Hour
        };
        await _context.TimeLogs.AddAsync(timeLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return timeLog.Id;
    }
}