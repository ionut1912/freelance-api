using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.Common;

[UsedImplicitly]
public record AddressData(
    string AddressCountry,
    string AddressStreet,
    string AddressStreetNumber,
    string AddressCity,
    string AddressZip);