using FluentValidation;

namespace Frelance.Application.Queries.Tasks.GetTaskById;

public class GetTaskByIdQueryValidator:AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}