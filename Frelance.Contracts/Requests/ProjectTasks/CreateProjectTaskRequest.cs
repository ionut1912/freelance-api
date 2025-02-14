using Frelance.Contracts.Enums;

namespace Frelance.Contracts.Requests.ProjectTasks;

public record CreateProjectTaskRequest(string ProjectTitle,string FreelancerUsername, string Title, string Description, string Priority);
