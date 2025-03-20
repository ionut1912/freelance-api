using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Application.Repositories;

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