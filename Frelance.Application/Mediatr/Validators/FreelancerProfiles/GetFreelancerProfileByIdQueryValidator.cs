using FluentValidation;
using Frelance.Application.Mediatr.Queries.FreelancerProfiles;

namespace Frelance.Application.Mediatr.Validators.FreelancerProfiles;

public class GetFreelancerProfileByIdQueryValidator : AbstractValidator<GetFreelancerProfileByIdQuery>
{
    public GetFreelancerProfileByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

}