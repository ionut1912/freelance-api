


using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Contracts.Responses.Tasks;

public record GetTasksResponse(PaginatedList<TaskDto> Results);