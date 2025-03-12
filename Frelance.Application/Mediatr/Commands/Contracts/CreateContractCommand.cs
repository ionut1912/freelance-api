using Frelance.Contracts.Requests.Contracts;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Contracts;

[UsedImplicitly]
public record CreateContractCommand(CreateContractRequest CreateContractRequest) : IRequest;