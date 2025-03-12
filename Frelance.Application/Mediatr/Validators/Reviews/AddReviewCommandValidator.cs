using FluentValidation;
using Frelance.Application.Mediatr.Commands.Reviews;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Reviews;

[UsedImplicitly]
public class AddReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.CreateReviewRequest.ReviewText).NotEmpty();
    }
}