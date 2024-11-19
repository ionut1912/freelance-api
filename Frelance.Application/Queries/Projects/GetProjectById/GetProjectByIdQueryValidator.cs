using FluentValidation;

namespace Frelance.Application.Queries.Projects.GetProjectById;

public class GetProjectByIdQueryValidator:AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}