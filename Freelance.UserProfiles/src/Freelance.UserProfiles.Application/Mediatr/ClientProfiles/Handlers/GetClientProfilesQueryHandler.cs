using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public sealed class GetClientProfilesQueryHandler(IClientProfileRepository clientProfileRepository)
{
    public async Task<List<ClientProfileDto>> Handle(GetClientProfilesQuery query, CancellationToken cancellationToken)
    {
        var clientProfiles = await clientProfileRepository.GetAllAsync(cancellationToken);
        return clientProfiles.ToDtos();
    }
}
