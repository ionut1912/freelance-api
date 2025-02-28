using Frelance.Contracts.Requests.Contracts;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Contracts;

public record CreateContractCommand(CreateContractRequest CreateContractRequest) : IRequest;