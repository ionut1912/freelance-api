namespace Frelance.API.Frelamce.Contracts;

public record GetTasksResponse(PaginatedList<TaskDto> Results);