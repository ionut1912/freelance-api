using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.ClientProfiles;

public sealed class UpdateClientProfileAddressCommandValidator : AbstractValidator<UpdateClientProfileAddressCommand>
{
    public UpdateClientProfileAddressCommandValidator()
    {
        RuleFor(x => x.AddressDto).NotEmpty().WithMessage("Address is required");
    }
}