using Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.ValueObjects;

public class Address : ValueObject
{
    private Address()
    {
    } // For EF Core

    private Address(string street, string city, string state, string zipCode, string country, string streetNumber)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        StreetNumber = streetNumber ?? throw new ArgumentNullException(nameof(streetNumber));
    }

    public string Street { get; } = string.Empty;
    public string City { get; }=string.Empty;
    public string State { get; } = string.Empty;
    public string StreetNumber { get; } = string.Empty;
    public string ZipCode { get; } = string.Empty;
    public string Country { get; } = string.Empty;

    public static Address Create(string street, string city, string state, string zipCode, string country,
        string streetNumber)
    {
        return new Address(street, city, state, zipCode, country, streetNumber);
    }

    public static Address Update(string street, string city, string state, string zipCode, string country,
        string streetNumber)
    {
        return new Address(street, city, state, zipCode, country, streetNumber);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
        yield return Country;
        yield return StreetNumber;
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {State} {ZipCode}, {Country}, {StreetNumber}";
    }
}