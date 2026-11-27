using Freelance.UserProfiles.Domain.ValueObjects;
using FluentValidation;
using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Validators;

/// <summary>Mirrors the column constraints of the owned address. Empty parts are allowed, missing ones are not.</summary>
public sealed class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street).NotNull().MaximumLength(Address.StreetMaxLength);
        RuleFor(x => x.City).NotNull().MaximumLength(Address.CityMaxLength);
        RuleFor(x => x.State).NotNull().MaximumLength(Address.StateMaxLength);
        RuleFor(x => x.ZipCode).NotNull().MaximumLength(Address.ZipCodeMaxLength);
        RuleFor(x => x.Country).NotNull().MaximumLength(Address.CountryMaxLength);
        RuleFor(x => x.StreetNumber).NotNull().MaximumLength(Address.StreetNumberMaxLength);
    }
}
