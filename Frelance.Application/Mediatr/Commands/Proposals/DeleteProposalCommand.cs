using MediatR;

namespace Frelance.Application.Mediatr.Commands.Proposals;

public record DeleteProposalCommand(int Id) : IRequest;