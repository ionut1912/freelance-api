using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Mapster;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Projects;

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