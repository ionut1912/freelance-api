using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Dtos;

public record ClientProfileDto(
    string AddressCountry,
    string AddressStreet,
    string AddressStreetNumber,
    string AddressCity,
    string AddressZip,
    string Bio,
    IFormFile ProfileImage);
