using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelance.Domain.Entities;
using Frelance.API.Frelance.Infrastructure;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.API.Frelance.Application.Queries.Projects.GetProjectById;

public class GetProjectByIdQueryHandler:IRequestHandler<GetProjectByIdQuery,GetProjectByIdResponse>
{
    private readonly FrelanceDbContext _context;

    public GetProjectByIdQueryHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<GetProjectByIdResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project=await _context.Projects.FirstOrDefaultAsync(x=>x.Id==request.Id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"{nameof(Project)} with {nameof(Project.Id)} : '{request.Id}' does not exist");
        }

        return project.Adapt<GetProjectByIdResponse>();
    }
}