using FluentValidation;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelancer.UserProfiles.Application.Validators.FreelancerProfiles;

public class UpdateRatingCommandValidator : AbstractValidator<UpdateFreelancerRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.Rating).NotEmpty().WithMessage("Rating is required");
    }
}