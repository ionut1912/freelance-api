using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Tasks;

public record CreateTaskCommand(string ProjectTitle,string Title,string Description,Priority Priority) : IRequest<int>;