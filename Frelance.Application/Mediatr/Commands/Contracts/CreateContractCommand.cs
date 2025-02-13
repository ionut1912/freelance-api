using Frelance.Contracts.Requests.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.Contracts;

public record CreateContractCommand(CreateContractRequest CreateContractRequest):IRequest<Unit>;