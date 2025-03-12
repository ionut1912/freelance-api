using JetBrains.Annotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
public record AddressDto(int Id, string Country, string City, string Street, string StreetNumber, string ZipCode);