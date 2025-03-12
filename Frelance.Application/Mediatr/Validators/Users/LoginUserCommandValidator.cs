using FluentValidation;
using Frelance.Application.Mediatr.Commands.Users;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Users;

[UsedImplicitly]
public class LoginUserCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.LoginDto.Password).NotEmpty();
        RuleFor(x => x.LoginDto.Username).NotEmpty();
        RuleFor(x => x.LoginDto.Email).NotEmpty().EmailAddress();
    }
}