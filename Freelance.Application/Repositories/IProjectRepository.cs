using Freelance.Application.Mediatr.Commands.Projects;
using Freelance.Application.Mediatr.Queries.Projects;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface IProjectRepository
{
    Task CreateProjectAsync(CreateProjectCommand createProjectCommand, CancellationToken cancellationToken);
    Task UpdateProjectAsync(UpdateProjectCommand updateProjectCommand, CancellationToken cancellationToken);
    Task DeleteProjectAsync(DeleteProjectCommand deleteProjectCommand, CancellationToken cancellationToken);
    Task<ProjectDto> FindProjectByIdAsync(GetProjectByIdQuery getProjectByIdQuery, CancellationToken cancellationToken);

    Task<PaginatedList<ProjectDto>> FindProjectsAsync(GetProjectsQuery getProjectsQuery,
        CancellationToken cancellationToken);
}