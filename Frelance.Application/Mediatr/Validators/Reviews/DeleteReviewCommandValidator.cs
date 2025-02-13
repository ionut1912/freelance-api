using FluentValidation;
using Frelance.Application.Mediatr.Commands.Reviews;

namespace Frelance.Application.Mediatr.Validators.Reviews;

public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}