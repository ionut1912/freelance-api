using FluentValidation;
using Frelance.Application.Mediatr.Queries.Projects;

namespace Frelance.Application.Mediatr.Validators.Projects;

public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}