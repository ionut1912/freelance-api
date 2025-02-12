namespace Frelance.Contracts.Requests.Address;

public record AddressRequest(string Country, string City, string Street, string StreetNumber, string ZipCode);