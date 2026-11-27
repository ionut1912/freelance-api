using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Validators;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class AddressDtoValidatorTests
{
    private readonly AddressDtoValidator _validator = new();

    private static string Text(int length) => new('a', length);

    private static AddressDto WithLength(string field, int length) => field switch
    {
        nameof(AddressDto.Street) => Profiles.Address with { Street = Text(length) },
        nameof(AddressDto.City) => Profiles.Address with { City = Text(length) },
        nameof(AddressDto.State) => Profiles.Address with { State = Text(length) },
        nameof(AddressDto.ZipCode) => Profiles.Address with { ZipCode = Text(length) },
        nameof(AddressDto.Country) => Profiles.Address with { Country = Text(length) },
        _ => Profiles.Address with { StreetNumber = Text(length) }
    };

    [Theory]
    [InlineData(nameof(AddressDto.Street), 200)]
    [InlineData(nameof(AddressDto.City), 100)]
    [InlineData(nameof(AddressDto.State), 100)]
    [InlineData(nameof(AddressDto.ZipCode), 20)]
    [InlineData(nameof(AddressDto.Country), 100)]
    [InlineData(nameof(AddressDto.StreetNumber), 100)]
    public void Validate_LengthBoundary_MatchesTheColumn(string field, int maximum)
    {
        _validator.Validate(WithLength(field, maximum)).IsValid.Should().BeTrue();
        _validator.Validate(WithLength(field, maximum + 1)).Errors.Should().ContainSingle(e => e.PropertyName == field);
    }

    [Fact]
    public void Validate_MissingParts_ReportsEachOfThem() =>
        _validator.Validate(new AddressDto(null!, null!, null!, null!, null!, null!)).Errors.Should().HaveCount(6);

    [Fact]
    public void Validate_EmptyParts_Passes() =>
        _validator.Validate(new AddressDto("", "", "", "", "", "")).IsValid.Should().BeTrue();
}
