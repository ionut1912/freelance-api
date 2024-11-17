namespace Frelance.API.Frelamce.Contracts.Projects;

public record UpdateProjectRequest(string Title,string Description,DateTime Deadline,List<string> Technologies,float Budget);