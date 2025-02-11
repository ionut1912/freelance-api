using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.ClientProfiles;

public record GetClientProfileByIdQuery(int Id) : IRequest<ClientProfileDto>;