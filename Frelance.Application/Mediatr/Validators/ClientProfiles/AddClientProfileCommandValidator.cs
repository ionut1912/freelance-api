using FluentValidation;
using Frelance.Application.Mediatr.Commands.ClientProfiles;

namespace Frelance.Application.Mediatr.Validators.ClientProfiles;

public class AddClientProfileCommandValidator : AbstractValidator<CreateClientProfileCommand>
{
    public AddClientProfileCommandValidator()
    {
        RuleFor(x => x.CreateClientProfileRequest.AddressCountry).NotEmpty();
        RuleFor(x => x.CreateClientProfileRequest.AddressCity).NotEmpty();
        RuleFor(x => x.CreateClientProfileRequest.AddressStreet).NotEmpty();
        RuleFor(x => x.CreateClientProfileRequest.AddressStreetNumber).NotEmpty();
        RuleFor(x => x.CreateClientProfileRequest.AddressZip).NotEmpty();
        RuleFor(x => x.CreateClientProfileRequest.Bio).NotEmpty();

    }

}