using FluentValidation;
using Frelance.Application.Mediatr.Commands.Reviews;

namespace Frelance.Application.Mediatr.Validators.Reviews;

public class AddReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.CreateReviewRequest.ReviewText).NotEmpty();
    }
}