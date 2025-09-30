using FluentValidation;
using Freelance.Application.Mediatr.Commands.Contracts;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Contracts;

[UsedImplicitly]
public class DeleteContractCommandValidator : AbstractValidator<DeleteContractCommand>
{
    public DeleteContractCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}