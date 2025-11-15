using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Query;

namespace Freelance.Identity.Application.Validators;

public class GetCurrentAccountQueryValidator : AbstractValidator<GetCurrentAccountQuery>
{
    public GetCurrentAccountQueryValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
    }
}