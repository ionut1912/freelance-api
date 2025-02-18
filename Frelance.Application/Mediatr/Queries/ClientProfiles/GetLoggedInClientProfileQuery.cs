using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.ClientProfiles;

public record GetLoggedInClientProfileQuery() : IRequest<ClientProfileDto?>;