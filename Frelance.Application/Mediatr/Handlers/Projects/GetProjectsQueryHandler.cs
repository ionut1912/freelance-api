using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Projects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, PaginatedList<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(IProjectRepository projectRepository)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        _projectRepository = projectRepository;
    }

    public async Task<PaginatedList<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        return await _projectRepository.FindProjectsAsync(request, cancellationToken);
    }
}