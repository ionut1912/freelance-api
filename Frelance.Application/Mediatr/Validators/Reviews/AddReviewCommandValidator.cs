using FluentValidation;
using Frelance.Application.Mediatr.Commands.Reviews;

namespace Frelance.Application.Mediatr.Validators.Reviews;

public class AddReviewCommandValidator:AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x=>x.ReviewText).NotEmpty();
    }
}