using FluentValidation;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

namespace Freelancer.UserProfiles.Application.Validators.ClientProfiles;

public class UpdateClientProfileDataCommandValidator : AbstractValidator<UpdateClientProfileDataCommand>
{
    public UpdateClientProfileDataCommandValidator()
    {
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required");
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
    }
}