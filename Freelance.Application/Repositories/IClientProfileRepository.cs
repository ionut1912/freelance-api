using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.ClientProfile;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface IClientProfileRepository
{
    Task CreateClientProfileAsync(CreateClientProfileRequest createClientProfileRequest,
        CancellationToken cancellationToken);

    Task<ClientProfileDto> GetClientProfileByIdAsync(int id,
        CancellationToken cancellationToken);

    Task<ClientProfileDto> GetLoggedInClientProfileAsync(CancellationToken cancellationToken);

    Task<PaginatedList<ClientProfileDto>> GetClientProfilesAsync(PaginationParams paginationParams,
        CancellationToken cancellationToken);

    Task PatchAddressAsync(PatchAddressCommand patchAddressCommand, CancellationToken cancellationToken);
    Task PatchUserDetailsAsync(PatchUserDetailsCommand patchUserDetailsCommand, CancellationToken cancellationToken);
    Task VerifyProfileAsync(int id, CancellationToken cancellationToken);
    Task DeleteClientProfileAsync(int id, CancellationToken cancellationToken);
}