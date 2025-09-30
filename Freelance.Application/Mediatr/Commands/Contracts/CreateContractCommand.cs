using Freelance.Contracts.Requests.Contracts;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Contracts;

[UsedImplicitly]
public record CreateContractCommand(CreateContractRequest CreateContractRequest) : IRequest;