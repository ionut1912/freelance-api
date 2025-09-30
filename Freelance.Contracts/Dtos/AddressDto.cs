using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record AddressDto(int Id, string Country, string City, string Street, string StreetNumber, string ZipCode);