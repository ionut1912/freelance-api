using FluentValidation;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

namespace Freelancer.UserProfiles.Application.Validators.ClientProfiles;

public class UpdateClientProfileAddressCommandValidator : AbstractValidator<UpdateClientProfileAddressCommand>
{
    public UpdateClientProfileAddressCommandValidator()
    {
        RuleFor(x => x.AddressDto).NotEmpty().WithMessage("Address is required");
    }
}