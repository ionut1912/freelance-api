using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

public sealed class UpdateFreelancerProfileAddressCommandValidator : AbstractValidator<UpdateFreelancerProfileAddressCommand>
{
    public UpdateFreelancerProfileAddressCommandValidator()
    {
        RuleFor(x => x.AddressDto).NotEmpty().WithMessage("Address is required")
            .SetValidator(new AddressDtoValidator());
    }
}