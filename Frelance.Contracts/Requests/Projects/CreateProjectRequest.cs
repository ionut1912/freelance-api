namespace Frelance.Contracts.Requests.Projects;

public record CreateProjectRequest(
    string Title,
    string Description,
    DateTime Deadline,
    List<string> Technologies,
    decimal Budget);