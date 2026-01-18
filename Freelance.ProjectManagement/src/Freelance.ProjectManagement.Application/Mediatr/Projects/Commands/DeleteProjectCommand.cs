
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

public record DeleteProjectCommand(Guid Id) : IRequest
{
}
