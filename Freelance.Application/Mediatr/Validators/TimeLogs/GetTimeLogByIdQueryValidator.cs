using FluentValidation;
using Freelance.Application.Mediatr.Queries.TimeLogs;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.TimeLogs;

[UsedImplicitly]
public class GetTimeLogByIdQueryValidator : AbstractValidator<GetTimeLogByIdQuery>
{
    public GetTimeLogByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}