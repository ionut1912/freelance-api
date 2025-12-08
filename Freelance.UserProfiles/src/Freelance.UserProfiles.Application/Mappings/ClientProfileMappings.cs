using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Requests;
using Freelance.UserProfiles.Domain.Entities;

namespace Freelance.UserProfiles.Application.Mappings;

public static class ClientProfileMappings
{
    public static ClientProfileDto ToDto(this ClientProfile clientProfile)
    {

        return new ClientProfileDto(clientProfile.AccountId,
            new AddressDto(clientProfile.Address.Street, clientProfile.Address.City, clientProfile.Address.Country, clientProfile.Address.StreetNumber, clientProfile.Address.ZipCode, clientProfile.Address.State),
            clientProfile.Bio,
            clientProfile.Image,
            clientProfile.IsVerified
        );
    }

    public static List<ClientProfileDto> ToDtos(this IEnumerable<ClientProfile> clientProfiles)
        => [.. clientProfiles.Select(cp => cp.ToDto())];

    public static CreateClientProfileCommand ToCreateCommand(this CreateClientProfileRequest createClientProfileRequest, Guid accountId)
    {
        return new CreateClientProfileCommand(accountId, createClientProfileRequest.Address, createClientProfileRequest.Bio, createClientProfileRequest.Image);
    }

    public static UpdateClientProfileAddressCommand ToUpdateClientAddressCommand(this UpdateProfileAddressRequest updateClientProfileAddressRequest, Guid id)
    {
        return new UpdateClientProfileAddressCommand(id, updateClientProfileAddressRequest.AddressDto);
    }

    public static UpdateClientProfileDataCommand ToUpdateClientDataCommand(this UpdateProfileDataRequest updateProfileDataRequest, Guid id)
    {
        return new UpdateClientProfileDataCommand(id, updateProfileDataRequest.Bio, updateProfileDataRequest.Image);
    }
}

