using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Contracts.Exceptions;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Mediatr.Handlers.TimeLogs;

public class DeleteTimeLogCommandHandler:IRequestHandler<DeleteTimeLogCommand,Unit>
{
    private readonly FrelanceDbContext _context;

    public DeleteTimeLogCommandHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteTimeLogCommand request, CancellationToken cancellationToken)
    {
        var timeLogToRemove=await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==request.Id, cancellationToken);
        if (timeLogToRemove is null)
        {
            throw new NotFoundException($"{nameof(TimeLog)} with {nameof(TimeLog.Id)} : '{request.Id}' does not exist");
        }
        _context.TimeLogs.Remove(timeLogToRemove);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}