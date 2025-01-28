using FluentValidation;
using Frelance.Application.Mediatr.Queries.Users;

namespace Frelance.Application.Mediatr.Validators.Users;

public class LoginUserQueryValidator:AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x=>x.Email).NotEmpty().EmailAddress();
    }
    
}