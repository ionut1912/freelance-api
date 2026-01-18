using Shared.Application.Mediator;

namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

public record UpdateProjectCommand(Guid Id, string Title, string Description, DateTime Deadline, decimal Amount, string Currency, List<string> Technologies) : IRequest;
