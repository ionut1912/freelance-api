using FluentValidation;
using Frelance.Application.Mediatr.Commands.Contracts;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Contracts;

[UsedImplicitly]
public class DeleteContractCommandValidator : AbstractValidator<DeleteContractCommand>
{
    public DeleteContractCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}