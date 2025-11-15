using FluentValidation;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

namespace Freelancer.UserProfiles.Application.Validators.ClientProfiles;

public class CreateClientProfileCommandValidator : AbstractValidator<CreateClientProfileCommand>
{
    public CreateClientProfileCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("AccountId is required");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required");
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
    }
}