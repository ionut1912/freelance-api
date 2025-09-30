using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.TimeLogs;

public record GetTimeLogsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<TimeLogDto>>;