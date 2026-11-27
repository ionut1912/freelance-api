using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

public sealed class CreateFreelancerProfileCommandValidator : AbstractValidator<CreateFreelancerProfileCommand>
{
    public CreateFreelancerProfileCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account Id is required");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required")
            .SetValidator(new AddressDtoValidator());
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required")
            .MaximumLength(100);
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
        RuleFor(x => x.Experience).NotEmpty().WithMessage("Experience is required");
        RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency should be 3 characters length");
        RuleFor(x => x.PortfolioUrl).NotEmpty().WithMessage("PortfolioUrl is required");
        RuleFor(x => x.ForeignLanguages).NotEmpty().WithMessage("ForeignLanguages is required");
        RuleFor(x => x.ProgrammingLanguages).NotEmpty().WithMessage("ProgrammingLanguages is required");
        RuleFor(x => x.Areas).NotEmpty().WithMessage("Areas is required");
        RuleForEach(x => x.ForeignLanguages).NotEmpty().MaximumLength(100);
        RuleForEach(x => x.ProgrammingLanguages).NotEmpty();
        RuleForEach(x => x.Areas).NotEmpty();

        RuleFor(x => x)
            .Custom((x, context) =>
            {
                if (x.ProgrammingLanguages?.Count != x.Areas?.Count)
                    context.AddFailure("ProgrammingLanguages and Areas must have the same number of items.");
            });
    }
}