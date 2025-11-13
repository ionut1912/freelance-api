using FluentValidation;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelancer.UserProfiles.Application.Validators.FreelancerProfiles;

public class UpdateFreelancerProfileAddressCommandValidator:AbstractValidator<UpdateFreelancerProfileAddressCommand>
{
    public UpdateFreelancerProfileAddressCommandValidator()
    {
        RuleFor(x=>x.AddressDto).NotEmpty().WithMessage("Address is required");
    }
}