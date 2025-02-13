using Frelance.Contracts.Requests.Contracts;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Contracts;

public record UpdateContractCommand(int Id,UpdateContractRequest UpdateContractRequest):IRequest<Unit>;