using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.Interfaces;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;

public sealed class GetProjectByIdQueryHandler(IProjectRepository projectRepository)
{
    public async Task<ProjectDto> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(query.Id, cancellationToken, p => p.Tasks, p => p.Technologies)
                      ?? throw new ProjectNotFoundException($"Project with id {query.Id} was not found");

        return project.ToDto();
    }
}
