using MediatR;

namespace Frelance.Application.Mediatr.Commands.Contracts;

public record DeleteContractCommand(int Id) : IRequest;