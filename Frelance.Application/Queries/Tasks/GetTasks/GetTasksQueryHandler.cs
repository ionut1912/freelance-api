using Frelance.Application.Helpers;
using MediatR;

namespace Frelance.Application.Queries.Tasks.GetTasks;
using Microsoft.EntityFrameworkCore;
using Mapster;
public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PaginatedList<TaskDto>>
{
    private readonly FrelanceDbContext _context;

    public GetTasksQueryHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var taskQuery = _context.Tasks.AsNoTracking().Include(x=>x.Project).ProjectToType<TaskDto>().AsQueryable();
        return await CollectionHelper<TaskDto>.ToPaginatedList(taskQuery,request.PaginationParams.PageNumber,request.PaginationParams.PageSize);
    }
}

