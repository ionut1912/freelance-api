using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.ClientProfile;

[UsedImplicitly]
public class UpdateClientProfileRequest(
    string addressCountry,
    string addressStreet,
    string addressStreetNumber,
    string addressCity,
    string addressZip,
    string bio,
    string image)
{
    public string? AddressCountry { get; } = addressCountry;
    public string? AddressStreet { get; } = addressStreet;
    public string? AddressStreetNumber { get; } = addressStreetNumber;
    public string? AddressCity { get; } = addressCity;
    public string? AddressZip { get; } = addressZip;
    public string? Bio { get; } = bio;
    public string? Image { get; } = image;
}