using FluentValidation;
using Frelance.Application.Mediatr.Queries.TimeLogs;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

[UsedImplicitly]
public class GetTimeLogByIdQueryValidator : AbstractValidator<GetTimeLogByIdQuery>
{
    public GetTimeLogByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}