using Frelance.Contracts.Requests.Proposals;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Proposals;

public record CreateProposalCommand(CreateProposalRequest CreateProposalRequest) : IRequest<Unit>;