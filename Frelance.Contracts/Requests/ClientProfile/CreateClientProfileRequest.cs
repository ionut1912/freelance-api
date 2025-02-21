namespace Frelance.Contracts.Requests.ClientProfile;

public record CreateClientProfileRequest(
    string AddressCountry,
    string AddressStreet,
    string AddressStreetNumber,
    string AddressCity,
    string AddressZip,
    string Bio,
    string Image);