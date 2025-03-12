using Frelance.Contracts.Requests.Proposals;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Proposals;

[UsedImplicitly]
public record UpdateProposalCommand(int Id, UpdateProposalRequest UpdateProposalRequest) : IRequest;