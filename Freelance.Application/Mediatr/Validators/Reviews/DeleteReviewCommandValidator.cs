using FluentValidation;
using Freelance.Application.Mediatr.Commands.Reviews;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Reviews;

[UsedImplicitly]
public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}