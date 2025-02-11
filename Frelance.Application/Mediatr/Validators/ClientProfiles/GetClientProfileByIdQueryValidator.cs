using FluentValidation;
using Frelance.Application.Mediatr.Queries.ClientProfiles;

namespace Frelance.Application.Mediatr.Validators.ClientProfiles;

public class GetClientProfileByIdQueryValidator : AbstractValidator<GetClientProfileByIdQuery>
{
    public GetClientProfileByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}