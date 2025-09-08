using Frelance.Contracts.Requests.Common;
using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.ClientProfile;

[UsedImplicitly]
public record CreateClientProfileRequest(
    AddressData Address,
    UserData User);