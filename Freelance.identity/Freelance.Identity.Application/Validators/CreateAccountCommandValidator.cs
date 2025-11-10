using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;

namespace Freelance.Identity.Application.Validators;

public class CreateAccountCommandValidator:AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
      RuleFor(a=>a.Password).NotEmpty().WithMessage("Password is required");
      RuleFor(a=>a.Username).NotEmpty().WithMessage("Username is required");
      RuleFor(a=>a.Email).NotEmpty().WithMessage("Email is required")
          .EmailAddress()
          .WithMessage("Email has invalid format");
      RuleFor(x=>x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required")
          .MaximumLength(10)
          .WithMessage("Phone Number has invalid length");
      
      RuleFor(x=>x.Address).NotNull().WithMessage("Address is required");
      RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required")
          .Must(role => role == "Client" || role == "Freelancer")
          .WithMessage("Role must be either 'Client' or 'Freelancer'");;
    }
}