using MediatR;

namespace Frelance.Application.Mediatr.Commands.Projects;

public record UpdateProjectCommand(int Id, string Title, string Description, DateTime Deadline, List<string> Technologies, decimal Budget) : IRequest<Unit>;
