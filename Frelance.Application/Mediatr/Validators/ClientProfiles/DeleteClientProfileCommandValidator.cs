using FluentValidation;
using Frelance.Application.Mediatr.Commands.ClientProfiles;

namespace Frelance.Application.Mediatr.Validators.ClientProfiles;

public class DeleteClientProfileCommandValidator : AbstractValidator<DeleteClientProfileCommand>
{
    public DeleteClientProfileCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}