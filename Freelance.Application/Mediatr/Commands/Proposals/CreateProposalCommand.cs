using Freelance.Contracts.Requests.Proposals;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Proposals;

[UsedImplicitly]
public record CreateProposalCommand(CreateProposalRequest CreateProposalRequest) : IRequest;