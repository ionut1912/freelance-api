using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Projects;

[UsedImplicitly]
public record CreateProjectRequest(
    string Title,
    string Description,
    DateTime Deadline,
    List<string> Technologies,
    decimal Budget);