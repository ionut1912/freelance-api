namespace Freelance.ProjectManagement.Application.Dtos;

public record ProjectDto(Guid Id, string Title, string Description, DateTime Deadline, Guid FreelancerId, Guid ClientId, List<ProjectTechnologyDto> Technologies, List<ProjectTaskDto> ProjectTasks, decimal Amount, string Currency)
{
}
