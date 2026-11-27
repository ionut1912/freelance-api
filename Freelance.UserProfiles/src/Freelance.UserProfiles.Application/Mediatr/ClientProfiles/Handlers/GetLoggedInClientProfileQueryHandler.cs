using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public sealed class GetLoggedInClientProfileQueryHandler(IClientProfileRepository clientProfileRepository)
{
    public async Task<ClientProfileDto> Handle(GetLoggedInClientProfileQuery query, CancellationToken cancellationToken)
    {
        var clientProfile = await clientProfileRepository.GetLoggedInClientProfileAsync(query.AccountId, cancellationToken)
                            ?? throw new ProfileNotFoundException($"Profile with accountId {query.AccountId} not found");

        return clientProfile.ToDto();
    }
}
