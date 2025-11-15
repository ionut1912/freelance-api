using FluentValidation;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelancer.UserProfiles.Application.Validators.FreelancerProfiles;

public class UpdateProfileDataCommandValidator : AbstractValidator<UpdateFreelancerProfileDataCommand>
{
    public UpdateProfileDataCommandValidator()
    {
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required");
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
    }
}