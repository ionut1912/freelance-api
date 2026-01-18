using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.Application.Mappings;

public static class ProjectMappings
{
    public static ProjectDto ToDto(this Project project)
    {
        return new ProjectDto(
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

    public static List<ProjectDto> ToDtos(this IEnumerable<Project> projects)
    {
        return [.. projects.Select(p => p.ToDto())];
    }


    public static CreateProjectCommand ToCreateCommand(this CreateProjectRequest request)
    {
        return new CreateProjectCommand(request.Title, request.Description, request.Deadline, request.Amount, request.Currency, request.Technologies);
    }

    public static UpdateProjectCommand ToUpdateCommand(this UpdateProjectRequest request, Guid Id)
    {
        return new UpdateProjectCommand(Id,request.Title, request.Description, request.Deadline, request.Amount, request.Currency, request.Technologies);
    }
}
