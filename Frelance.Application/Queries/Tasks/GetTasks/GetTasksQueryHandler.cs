using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelance.Application.Helpers;
using Frelance.API.Frelance.Infrastructure;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.API.Frelance.Application.Queries.Tasks.GetTasks;

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

