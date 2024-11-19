using MediatR;

namespace Frelance.Application.Queries.Tasks.GetTasks;

public record GetTasksQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TaskDto>>;
