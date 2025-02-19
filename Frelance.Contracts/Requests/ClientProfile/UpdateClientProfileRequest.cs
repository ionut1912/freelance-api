using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.ClientProfile;

public class UpdateClientProfileRequest
{
    public string? AddressCountry { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressStreetNumber { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressZip { get; set; }
    public string? Bio { get; set; }
}