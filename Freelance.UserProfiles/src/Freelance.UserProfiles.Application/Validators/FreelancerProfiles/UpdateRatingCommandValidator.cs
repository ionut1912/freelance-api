using Freelance.UserProfiles.Domain.Entities;
using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

public sealed class UpdateRatingCommandValidator : AbstractValidator<UpdateFreelancerRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(FreelancerProfile.MinRating, FreelancerProfile.MaxRating)
            .WithMessage($"Rating must be between {FreelancerProfile.MinRating} and {FreelancerProfile.MaxRating}");
    }
}