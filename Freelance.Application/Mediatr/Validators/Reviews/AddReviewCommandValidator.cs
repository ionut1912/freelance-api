using FluentValidation;
using Freelance.Application.Mediatr.Commands.Reviews;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Reviews;

[UsedImplicitly]
public class AddReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.CreateReviewRequest.ReviewText).NotEmpty();
    }
}