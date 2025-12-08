using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

public class UpdateFreelancerDetailsCommandValidator : AbstractValidator<UpdateFreelancerDetailsCommand>
{
    public UpdateFreelancerDetailsCommandValidator()
    {
        RuleFor(x => x.ForeignLanguages).NotEmpty().WithMessage("Foreign languages are required");
        RuleFor(x => x.ProgrammingLanguages).NotEmpty().WithMessage("Programming languages are required");
        RuleFor(x => x.Areas).NotEmpty().WithMessage("Areas are required");
        RuleFor(x => x.Experience).NotEmpty().WithMessage("Experience is required");
        RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than zero");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency is required");
        RuleFor(x => x.PortfolioUrl).NotEmpty().WithMessage("Portfolio url is required");
        RuleFor(x => x)
            .Custom((x, context) =>
            {
                if (x.ProgrammingLanguages.Count != x.Areas.Count)
                    context.AddFailure("ProgrammingLanguages and Areas must have the same number of items.");
            });
    }
}