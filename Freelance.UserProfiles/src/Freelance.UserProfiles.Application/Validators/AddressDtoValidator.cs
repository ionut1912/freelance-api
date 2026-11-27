using FluentValidation;
using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Validators;

/// <summary>Mirrors the column constraints of the owned address. Empty parts are allowed, missing ones are not.</summary>
public sealed class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street).NotNull().MaximumLength(200);
        RuleFor(x => x.City).NotNull().MaximumLength(100);
        RuleFor(x => x.State).NotNull().MaximumLength(100);
        RuleFor(x => x.ZipCode).NotNull().MaximumLength(20);
        RuleFor(x => x.Country).NotNull().MaximumLength(100);
        RuleFor(x => x.StreetNumber).NotNull().MaximumLength(100);
    }
}
