namespace Frelance.API.Frelamce.Contracts.Projects;

public record CreateProjectRequest(string Title,string Description,DateTime Deadline,List<string> Technologies,float Budget);