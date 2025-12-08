using Freelance.UserProfiles.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;

public record GetLoggedInClientProfileQuery(Guid AccountId) : IRequest<ClientProfileDto>
{

}