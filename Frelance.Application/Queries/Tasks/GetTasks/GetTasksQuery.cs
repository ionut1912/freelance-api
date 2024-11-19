using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Queries.Tasks.GetTasks;

public record GetTasksQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TaskDto>>;
