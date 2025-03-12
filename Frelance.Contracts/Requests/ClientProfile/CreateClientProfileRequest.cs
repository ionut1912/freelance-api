using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.ClientProfile;

[UsedImplicitly]
public record CreateClientProfileRequest(
    string AddressCountry,
    string AddressStreet,
    string AddressStreetNumber,
    string AddressCity,
    string AddressZip,
    string Bio,
    string Image);