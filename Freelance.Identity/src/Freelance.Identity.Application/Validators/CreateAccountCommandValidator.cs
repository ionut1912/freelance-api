using Freelance.Identity.Domain.Entities;
using System.Text;
using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.ValueObjects;

namespace Freelance.Identity.Application.Validators;

public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    // BCrypt silently ignores everything after this many bytes.
    private const int MaxPasswordBytes = 72;

    public CreateAccountCommandValidator()
    {
        RuleFor(a => a.Password).NotEmpty().WithMessage("Password is required")
            .Must(password => password is null || Encoding.UTF8.GetByteCount(password) <= MaxPasswordBytes)
            .WithMessage($"Password can be at most {MaxPasswordBytes} bytes long");
        RuleFor(a => a.Username).NotEmpty().WithMessage("Username is required")
            .MaximumLength(Account.UsernameMaxLength);
        RuleFor(a => a.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email has invalid format")
            .MaximumLength(Account.EmailMaxLength);
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required")
            .MaximumLength(Account.PhoneNumberMaxLength).WithMessage("Phone Number has invalid length");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required")
            .Must(Role.IsValid).WithMessage("Role must be either 'Client' or 'Freelancer'");
    }
}