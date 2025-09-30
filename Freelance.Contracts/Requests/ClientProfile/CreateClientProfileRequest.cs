using Freelance.Contracts.Requests.Common;
using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.ClientProfile;

[UsedImplicitly]
public record CreateClientProfileRequest(
    AddressData Address,
    UserData User);