using FluentValidation;
using Freelance.Application.Mediatr.Queries.Projects;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Projects;

[UsedImplicitly]
public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}