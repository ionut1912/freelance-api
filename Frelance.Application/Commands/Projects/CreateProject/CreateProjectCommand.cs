using MediatR;

namespace Frelance.API.Frelance.Application.Commands.Projects.CreateProject;

public record CreateProjectCommand(string Title, string Description, DateTime Deadline,List<string> Technologies,float Budget) : IRequest<int>;