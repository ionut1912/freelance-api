using FluentValidation;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelancer.UserProfiles.Application.Validators.FreelancerProfiles;

public class CreateFreelancerProfileCommandValidator : AbstractValidator<CreateFreelancerProfileCommand>
{
    public CreateFreelancerProfileCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account Id is required");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required");
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
        RuleFor(x => x.Experience).NotEmpty().WithMessage("Experience is required");
        RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency is required");
        RuleFor(x => x.PortfolioUrl).NotEmpty().WithMessage("PortfolioUrl is required");
        RuleFor(x => x.ForeignLanguages).NotEmpty().WithMessage("ForeignLanguages is required");
        RuleFor(x => x.ProgrammingLanguages).NotEmpty().WithMessage("ProgrammingLanguages is required");
        RuleFor(x => x.Areas).NotEmpty().WithMessage("Areas is required");
        RuleFor(x => x)
            .Custom((x, context) =>
            {
                if (x.ProgrammingLanguages.Count != x.Areas.Count)
                    context.AddFailure("ProgrammingLanguages and Areas must have the same number of items.");
            });
    }
}