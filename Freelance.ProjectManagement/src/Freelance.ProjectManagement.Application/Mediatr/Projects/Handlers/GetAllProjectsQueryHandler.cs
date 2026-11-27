using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public sealed class GetAllProjectsQueryHandler(IProjectRepository projectRepository)
{
    public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken)
    {
        var projects = await projectRepository.GetAllAsync(cancellationToken, p => p.Tasks, p => p.Technologies);
        return projects.ToDtos();
    }
}
