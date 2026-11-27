using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.Application.Mappings;

public static class ProjectMappings
{
    extension(Project project)
    {
        public ProjectDto ToDto() => new(
            project.Id,
            project.Title,
            project.Description,
            project.Deadline,
            project.FreelancerId,
            project.ClientId,
            [.. project.Technologies.Select(t => new ProjectTechnologyDto(t.Technology))],
            project.Tasks.ToDtos(),
            project.Budget.Amount,
            project.Budget.Currency);
    }

    extension(IEnumerable<Project> projects)
    {
        public List<ProjectDto> ToDtos() => [.. projects.Select(p => p.ToDto())];
    }

    extension(CreateProjectRequest request)
    {
        public CreateProjectCommand ToCreateCommand() =>
            new(request.Title, request.Description, request.Deadline, request.Amount, request.Currency, request.Technologies);
    }

    extension(UpdateProjectRequest request)
    {
        public UpdateProjectCommand ToUpdateCommand(Guid id) =>
            new(id, request.Title, request.Description, request.Deadline, request.Amount, request.Currency, request.Technologies);
    }
}
