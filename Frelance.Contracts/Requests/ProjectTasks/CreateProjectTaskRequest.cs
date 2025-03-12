using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.ProjectTasks;

[UsedImplicitly]
public record CreateProjectTaskRequest(
    string ProjectTitle,
    string FreelancerUsername,
    string Title,
    string Description,
    string Priority);