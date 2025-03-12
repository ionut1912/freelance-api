using FluentValidation;
using Frelance.Application.Mediatr.Queries.Projects;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Projects;

[UsedImplicitly]
public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}