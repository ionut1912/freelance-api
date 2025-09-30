using Freelance.Contracts.Requests.Contracts;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Contracts;

[UsedImplicitly]
public record UpdateContractCommand(int Id, UpdateContractRequest UpdateContractRequest) : IRequest;