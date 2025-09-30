using FluentValidation;
using Freelance.Application.Mediatr.Queries.Tasks;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Tasks;

[UsedImplicitly]
public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}