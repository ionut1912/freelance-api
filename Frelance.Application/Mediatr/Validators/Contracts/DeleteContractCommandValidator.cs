using FluentValidation;
using Frelance.Application.Mediatr.Commands.Contracts;

namespace Frelance.Application.Mediatr.Validators.Contracts;

public class DeleteContractCommandValidator : AbstractValidator<DeleteContractCommand>
{
    public DeleteContractCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}