namespace Frelance.API.Frelance.Contracts.Requests.Projects;

public record CreateProjectRequest(string Title,string Description,DateTime Deadline,List<string> Technologies);