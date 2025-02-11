using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.ClientProfile;

public record UpdateClientProfileRequest(
    string AddressCountry,
    string AddressStreet,
    string AddressStreetNumber,
    string AddressCity,
    string AddressZip,
    string Bio,
    IFormFile ProfileImage);