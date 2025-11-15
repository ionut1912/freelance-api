using Freelancer.UserProfiles.Application.Dtos;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Queries;

public class GetLoggedInClientProfileQuery : IRequest<ClientProfileDto>
{
    public Guid AccountId { get; set; }
}