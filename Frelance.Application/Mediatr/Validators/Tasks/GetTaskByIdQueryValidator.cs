using FluentValidation;
using Frelance.Application.Mediatr.Queries.Tasks;

namespace Frelance.Application.Mediatr.Validators.Tasks;

public class GetTaskByIdQueryValidator:AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}