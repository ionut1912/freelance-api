using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

public sealed class UpdateProfileDataCommandValidator : AbstractValidator<UpdateFreelancerProfileDataCommand>
{
    public UpdateProfileDataCommandValidator()
    {
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required")
            .MaximumLength(100);
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
    }
}