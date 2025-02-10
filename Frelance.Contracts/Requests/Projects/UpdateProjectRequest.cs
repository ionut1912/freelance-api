namespace Frelance.Contracts.Requests.Projects;

public record UpdateProjectRequest(string Title, string Description, DateTime Deadline, List<string> Technologies, float Budget);