using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Query;

namespace Freelance.Identity.Application.Validators;

public sealed class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}