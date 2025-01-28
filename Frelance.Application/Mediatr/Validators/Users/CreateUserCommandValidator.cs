using System.Data;
using FluentValidation;
using Frelance.Application.Mediatr.Commands.Users;

namespace Frelance.Application.Mediatr.Validators.Users;

public class CreateUserCommandValidator:AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x=>x.RegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(x=>x.RegisterDto.Password).NotEmpty();
        RuleFor(x => x.RegisterDto.Username).NotEmpty();
        RuleFor(x=>x.RegisterDto.PhoneNumber).NotEmpty();
        RuleFor(x=>x.RegisterDto.Role).NotEmpty();
    }
    
}