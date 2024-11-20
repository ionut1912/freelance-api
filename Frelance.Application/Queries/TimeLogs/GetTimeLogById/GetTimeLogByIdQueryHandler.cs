using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.TimeLogs;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Queries.TimeLogs.GetTimeLogById;

public class GetTimeLogByIdQueryHandler:IRequestHandler<GetTimeLogByIdQuery, GetTimeLogByIdResponse>
{
    private readonly FrelanceDbContext _context;

    public GetTimeLogByIdQueryHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<GetTimeLogByIdResponse> Handle(GetTimeLogByIdQuery request, CancellationToken cancellationToken)
    {
        var timeLog= await _context.TimeLogs.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == request.Id,cancellationToken);
        if (timeLog is null)
        {
            throw new NotFoundException($"{nameof(TimeLog)} with {nameof(TimeLog.Id)} : '{request.Id}' does not exist");
        }
        return timeLog.Adapt<GetTimeLogByIdResponse>();
    }
}