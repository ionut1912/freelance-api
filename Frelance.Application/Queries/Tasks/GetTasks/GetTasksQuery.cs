using MediatR;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Dtos;
namespace Frelance.Application.Queries.Tasks.GetTasks;

public record GetTasksQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TaskDto>>;
