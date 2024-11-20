using FluentValidation;

namespace Frelance.Application.Queries.TimeLogs.GetTimeLogById;

public class GetTimeLogByIdQueryValidator: AbstractValidator<GetTimeLogByIdQuery>
{
    public GetTimeLogByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}