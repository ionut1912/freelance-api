using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Queries.TimeLogs.GetTimeLogs;

public record GetTimeLogsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TimeLogDto>>;