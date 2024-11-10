using MediatR;

namespace Frelance.API.Frelance.Application.Commands.Projects.UpdateProject;

public record UpdateProjectCommand(int Id,string? Title, string? Description, DateTime? Deadline,List<string>? Technologies) : IRequest<Unit>;