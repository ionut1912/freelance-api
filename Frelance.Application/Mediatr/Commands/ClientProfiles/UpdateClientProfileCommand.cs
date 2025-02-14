using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles;

public record UpdateClientProfileCommand(
    int Id, UpdateClientProfileRequest UpdateClientProfileRequest) : IRequest<Unit>;