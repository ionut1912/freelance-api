using Frelance.Contracts.Requests.Proposals;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Proposals;

public record UpdateProposalCommand(int Id, UpdateProposalRequest UpdateProposalRequest) : IRequest<Unit>;