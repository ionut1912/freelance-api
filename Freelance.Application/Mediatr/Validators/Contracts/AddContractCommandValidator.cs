using FluentValidation;
using Freelance.Application.Mediatr.Commands.Contracts;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Contracts;

[UsedImplicitly]
public class AddContractCommandValidator : AbstractValidator<CreateContractCommand>
{
    public AddContractCommandValidator()
    {
        RuleFor(x => x.CreateContractRequest.ProjectName).NotEmpty();
        RuleFor(x => x.CreateContractRequest.FreelancerName).NotEmpty();
        RuleFor(x => x.CreateContractRequest.StartDate).NotEmpty();
        RuleFor(x => x.CreateContractRequest.EndDate).NotEmpty();
        RuleFor(x => x.CreateContractRequest.Amount).NotEmpty();
    }
}