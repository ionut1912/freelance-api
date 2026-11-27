using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Requests;
using Freelance.UserProfiles.Domain.Entities;

namespace Freelance.UserProfiles.Application.Mappings;

public static class ClientProfileMappings
{
    extension(ClientProfile clientProfile)
    {
        public ClientProfileDto ToDto() => new(
            clientProfile.Id,
            clientProfile.AccountId,
            clientProfile.Address.ToDto(),
            clientProfile.Bio,
            clientProfile.Image,
            clientProfile.IsVerified);
    }

    extension(IEnumerable<ClientProfile> clientProfiles)
    {
        public List<ClientProfileDto> ToDtos() => [.. clientProfiles.Select(cp => cp.ToDto())];
    }

    extension(CreateClientProfileRequest request)
    {
        public CreateClientProfileCommand ToCreateCommand(Guid accountId) =>
            new(accountId, request.Address, request.Bio, request.Image);
    }

    extension(UpdateProfileAddressRequest request)
    {
        public UpdateClientProfileAddressCommand ToUpdateClientAddressCommand(Guid id, Guid accountId) =>
            new(id, request.AddressDto, accountId);
    }

    extension(UpdateProfileDataRequest request)
    {
        public UpdateClientProfileDataCommand ToUpdateClientDataCommand(Guid id, Guid accountId) =>
            new(id, request.Bio, request.Image, accountId);
    }

    extension(VerifyProfileRequest request)
    {
        public VerifyClientProfileCommand ToVerifyClientCommand(Guid accountId) => new(accountId, request.ImageUrl);
    }
}
