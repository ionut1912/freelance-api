using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Tasks;

public record GetTasksQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TaskDto>>;