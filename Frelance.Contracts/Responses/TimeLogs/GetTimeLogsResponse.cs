using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Contracts.Responses.TimeLogs;

public record GetTimeLogsResponse(PaginatedList<TimeLogDto> Results);