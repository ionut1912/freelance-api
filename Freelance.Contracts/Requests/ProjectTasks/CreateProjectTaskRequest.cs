using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.ProjectTasks;

[UsedImplicitly]
public record CreateProjectTaskRequest(
    string ProjectTitle,
    string FreelancerUsername,
    string Title,
    string Description,
    string Priority);