using FluentValidation;
using Frelance.Application.Mediatr.Commands.Reviews;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Reviews;

[UsedImplicitly]
public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}