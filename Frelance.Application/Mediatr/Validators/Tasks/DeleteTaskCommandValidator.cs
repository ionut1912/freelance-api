using FluentValidation;
using Frelance.Application.Mediatr.Commands.Tasks;

namespace Frelance.Application.Mediatr.Validators.Tasks;

public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}