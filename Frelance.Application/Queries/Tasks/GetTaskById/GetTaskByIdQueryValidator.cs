using FluentValidation;

namespace Frelance.API.Frelance.Application.Queries.Tasks.GetTaskById;

public class GetTaskByIdQueryValidator:AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}