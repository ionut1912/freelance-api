using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelance.Application.Helpers;
using Frelance.API.Frelance.Infrastructure;
using Mapster;
using MediatR;

namespace Frelance.Application.Queries.Projects.GetProjects;

public class GetProjectsQueryHandler:IRequestHandler<GetProjectsQuery,PaginatedList<ProjectDto>>
{
    private readonly FrelanceDbContext _context;

    public GetProjectsQueryHandler(FrelanceDbContext context)
    {
        _context = context;
        
    }

    public async Task<PaginatedList<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projectQuery = _context.Projects.ProjectToType<ProjectDto>().AsQueryable();
        return await CollectionHelper<ProjectDto>.ToPaginatedList(projectQuery,request.PaginationParams.PageNumber,request.PaginationParams.PageSize);
    }
}