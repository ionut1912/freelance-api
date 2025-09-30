using MediatR;

namespace Freelance.Application.Mediatr.Commands.Tasks;

public record DeleteTaskCommand(int Id) : IRequest;