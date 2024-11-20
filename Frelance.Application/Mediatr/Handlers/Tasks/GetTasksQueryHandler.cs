using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Mediatr.Handlers.Tasks;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PaginatedList<TaskDto>>
{
    private readonly FrelanceDbContext _context;

    public GetTasksQueryHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var taskQuery = _context.Tasks.AsNoTracking().Include(x=>x.Project).Include(x=>x.TimeLogs).ProjectToType<TaskDto>().AsQueryable();
        return await CollectionHelper<TaskDto>.ToPaginatedList(taskQuery,request.PaginationParams.PageNumber,request.PaginationParams.PageSize);
    }
}