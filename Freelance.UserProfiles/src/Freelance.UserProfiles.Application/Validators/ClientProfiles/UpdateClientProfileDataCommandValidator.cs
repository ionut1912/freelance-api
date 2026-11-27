using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.ClientProfiles;

public sealed class UpdateClientProfileDataCommandValidator : AbstractValidator<UpdateClientProfileDataCommand>
{
    public UpdateClientProfileDataCommandValidator()
    {
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required")
            .MaximumLength(100);
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
    }
}