using FluentValidation;
using Frelance.Application.Mediatr.Queries.Tasks;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Tasks;

[UsedImplicitly]
public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}