using FluentValidation;
using Frelance.Application.Mediatr.Commands.ClientProfiles;

namespace Frelance.Application.Mediatr.Validators.ClientProfiles;

public class AddClientProfileCommandValidator : AbstractValidator<AddClientProfileCommand>
{
    public AddClientProfileCommandValidator()
    {
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Bio).NotEmpty();
        RuleFor(x => x.ProfileImage).NotEmpty();

    }

}