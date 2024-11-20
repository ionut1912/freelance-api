using MediatR;

namespace Frelance.Application.Mediatr.Commands.Projects;

public record CreateProjectCommand(string Title, string Description, DateTime Deadline,List<string> Technologies,float Budget) : IRequest<int>;