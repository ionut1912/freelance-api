using FluentValidation;
using Frelance.Application.Mediatr.Queries.TimeLogs;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

public class GetTimeLogByIdQueryValidator: AbstractValidator<GetTimeLogByIdQuery>
{
    public GetTimeLogByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}