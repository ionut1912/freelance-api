using Freelance.ProjectManagement.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;

public record GetAllTimeLogsQuery : IRequest<List<TimeLogDto>>
{
}
