using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelance.Application.Queries.Projects.GetProjectById;
using Frelance.API.Frelance.Domain.Entities;
using Frelance.API.Frelance.Infrastructure;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.API.Frelance.Application.Queries.Tasks.GetTaskById;

public class GetTaskByIdQueryHandler:IRequestHandler<GetTaskByIdQuery,GetTaskByIdResponse>
{
    private readonly FrelanceDbContext _context;

    public GetTaskByIdQueryHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<GetTaskByIdResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.AsNoTracking().Include(x=>x.Project).FirstOrDefaultAsync(x=>x.Id==request.Id, cancellationToken);
        if (task is null)
        {
            throw new NotFoundException($"{nameof(ProjectTask)} with {nameof(ProjectTask.Id)} : '{request.Id}' does not exist");
        }
        return task.Adapt<GetTaskByIdResponse>();
    }
}