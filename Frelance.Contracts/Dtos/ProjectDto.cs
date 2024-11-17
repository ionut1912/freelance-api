namespace Frelance.API.Frelamce.Contracts;

public record ProjectDto(int Id,string Title,string Description,DateTime CreatedAt,DateTime Deadline,List<string> Technologies,float Budget);