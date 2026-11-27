using Freelance.UserProfiles.Domain.Entities;
using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.ClientProfiles;

public sealed class CreateClientProfileCommandValidator : AbstractValidator<CreateClientProfileCommand>
{
    public CreateClientProfileCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("AccountId is required");
        RuleFor(x => x.AddressDto).NotEmpty().WithMessage("Address is required")
            .SetValidator(new AddressDtoValidator());
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required")
            .MaximumLength(BaseUserProfile.BioMaxLength);
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
    }
}