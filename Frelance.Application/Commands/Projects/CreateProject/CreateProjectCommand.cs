using MediatR;

namespace Frelance.Application.Commands.Projects.CreateProject;

public record CreateProjectCommand(string Title, string Description, DateTime Deadline,List<string> Technologies) : IRequest<int>;