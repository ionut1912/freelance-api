using MediatR;

namespace Freelance.Application.Mediatr.Commands.Proposals;

public record DeleteProposalCommand(int Id) : IRequest;