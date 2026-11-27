using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.Application.Mappings;

public static class AddressMappings
{
    extension(Address address)
    {
        // Named arguments: the previous positional mapping swapped State/Country/ZipCode/StreetNumber.
        public AddressDto ToDto() => new(
            Street: address.Street,
            City: address.City,
            State: address.State,
            ZipCode: address.ZipCode,
            Country: address.Country,
            StreetNumber: address.StreetNumber);
    }
}
