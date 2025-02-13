using FluentValidation;
using Frelance.Application.Mediatr.Queries.Contracts;

namespace Frelance.Application.Mediatr.Validators.Contracts;

public class GetContractByIdQueryValidator:AbstractValidator<GetContractByIdQuery>
{
    public GetContractByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}