using FluentValidation;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;

namespace Frelance.Application.Mediatr.Validators.FreelancerProfiles;

public class AddFreelancerProfileCommandValidator : AbstractValidator<CreateFreelancerProfileCommand>
{
    public AddFreelancerProfileCommandValidator()
    {
        RuleFor(x => x.CreateFreelancerProfileRequest.AddressCountry).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.AddressCity).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.AddressStreet).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.AddressStreetNumber).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.AddressZip).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.Bio).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.Bio).MaximumLength(256);
        RuleFor(x => x.CreateFreelancerProfileRequest.Areas).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.ProgrammingLanguages).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.ForeignLanguages).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.Experience).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.Rate).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.Currency).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.Rating).NotEmpty();
        RuleFor(x => x.CreateFreelancerProfileRequest.PortfolioUrl).NotEmpty();
    }
}