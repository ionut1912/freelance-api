using FluentValidation;
using Frelance.Application.Mediatr.Commands.Contracts;
using System;

namespace Frelance.Application.Mediatr.Validators.Contracts
{
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
}