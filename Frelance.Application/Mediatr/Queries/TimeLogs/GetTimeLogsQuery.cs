using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.TimeLogs;

public record GetTimeLogsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TimeLogDto>>;