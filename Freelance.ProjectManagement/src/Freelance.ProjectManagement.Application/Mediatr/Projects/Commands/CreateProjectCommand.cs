using Freelance.ProjectManagement.Domain.Entities;
using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

public record CreateProjectCommand(string Title, string Description, DateTime Deadline, decimal Amount, string Currency, List<string> Technologies) : IRequest<Project>
{
}
