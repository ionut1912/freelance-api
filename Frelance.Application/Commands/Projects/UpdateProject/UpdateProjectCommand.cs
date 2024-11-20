using MediatR;

namespace Frelance.Application.Commands.Projects.UpdateProject;

public record UpdateProjectCommand(int Id,string Title, string Description, DateTime Deadline,List<string> Technologies,float Budget) : IRequest<Unit>;