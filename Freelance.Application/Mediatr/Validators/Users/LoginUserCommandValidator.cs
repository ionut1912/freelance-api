using FluentValidation;
using Freelance.Application.Mediatr.Commands.Users;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Users;

[UsedImplicitly]
public class LoginUserCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.LoginDto.Password).NotEmpty();
        RuleFor(x => x.LoginDto.Username).NotEmpty();
    }
}