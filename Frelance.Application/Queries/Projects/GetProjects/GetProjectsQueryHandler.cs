using Frelance.API.Frelance.Contracts.Dtos;
using Frelance.API.Frelance.Contracts.Responses;
using Frelance.Application.Helpers;
using Frelance.Infrastructure;
using Mapster;
using MediatR;

namespace Frelance.API.Frelance.Application.Queries.Projects.GetProjects;

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