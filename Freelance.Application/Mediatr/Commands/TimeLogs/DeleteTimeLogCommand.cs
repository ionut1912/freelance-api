using MediatR;

namespace Freelance.Application.Mediatr.Commands.TimeLogs;

public record DeleteTimeLogCommand(int Id) : IRequest;