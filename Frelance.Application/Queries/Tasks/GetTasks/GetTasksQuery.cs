using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelamce.Contracts.Common;
using MediatR;

namespace Frelance.API.Frelance.Application.Queries.Tasks.GetTasks;

public record GetTasksQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TaskDto>>;
