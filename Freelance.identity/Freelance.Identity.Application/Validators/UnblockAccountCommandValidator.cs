using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;

namespace Freelance.Identity.Application.Validators;

public class UnblockAccountCommandValidator:AbstractValidator<UnblockAccountCommand>
{
    public UnblockAccountCommandValidator()
    {
        RuleFor(x=>x.AccountId).NotEmpty().WithMessage("Account Id is required");
    }
    
}