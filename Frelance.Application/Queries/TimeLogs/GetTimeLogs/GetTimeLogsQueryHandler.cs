using Frelance.Application.Helpers;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Mapster;
using MediatR;

namespace Frelance.Application.Queries.TimeLogs.GetTimeLogs;

public class GetTimeLogsQueryHandler : IRequestHandler<GetTimeLogsQuery,PaginatedList<TimeLogDto>>
{
    private readonly FrelanceDbContext _context;

    public GetTimeLogsQueryHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<TimeLogDto>> Handle(GetTimeLogsQuery request, CancellationToken cancellationToken)
    {
        var timeLogsQuery = _context.TimeLogs.ProjectToType<TimeLogDto>().AsQueryable();
        return await CollectionHelper<TimeLogDto>.ToPaginatedList(timeLogsQuery,request.PaginationParams.PageNumber,request.PaginationParams.PageSize);
    }
}