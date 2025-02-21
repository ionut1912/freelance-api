using FluentValidation;
using Frelance.Application.Mediatr.Commands.Users;

namespace Frelance.Application.Mediatr.Validators.Users;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.RegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RegisterDto.Password).NotEmpty();
        RuleFor(x => x.RegisterDto.Username).NotEmpty();
        RuleFor(x => x.RegisterDto.PhoneNumber).NotEmpty()
            .Matches(@"^\d+$").WithMessage("Phone number must contain only digits (0-9).")
            .Length(10).WithMessage("Phone number must be exactly 10 digits.");
        RuleFor(x => x.RegisterDto.Role).NotEmpty()
            .Must(r => r is "Freelancer" or "Client").WithMessage("Role must be either 'Freelancer' or 'Client'");
    }
}