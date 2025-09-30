using MediatR;

namespace Freelance.Application.Mediatr.Commands.Contracts;

public record DeleteContractCommand(int Id) : IRequest;