using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;

namespace Freelance.Identity.Application.Validators;

public class BlockAccountCommandValidator : AbstractValidator<BlockAccountCommand>
{
    public BlockAccountCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account Id is required");
    }
}