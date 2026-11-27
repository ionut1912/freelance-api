using System.Text;
using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.ValueObjects;

namespace Freelance.Identity.Application.Validators;

public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(a => a.Password).NotEmpty().WithMessage("Password is required")
            // BCrypt silently ignores everything after 72 bytes.
            .Must(password => password is null || Encoding.UTF8.GetByteCount(password) <= 72)
            .WithMessage("Password can be at most 72 bytes long");
        RuleFor(a => a.Username).NotEmpty().WithMessage("Username is required")
            .MaximumLength(50);
        RuleFor(a => a.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email has invalid format")
            .MaximumLength(100);
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required")
            .MaximumLength(10).WithMessage("Phone Number has invalid length");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required")
            .Must(Role.IsValid).WithMessage("Role must be either 'Client' or 'Freelancer'");
    }
}