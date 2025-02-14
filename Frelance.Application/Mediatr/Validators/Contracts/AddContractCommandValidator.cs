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
            RuleFor(x => x.CreateContractRequest.ContractFile).NotEmpty();
            RuleFor(x => x.CreateContractRequest.ContractFile)
                .Must(file =>
                    file != null &&
                    (file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase)
                     || file.ContentType.Equals("application/msword", StringComparison.OrdinalIgnoreCase)
                     || file.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document", StringComparison.OrdinalIgnoreCase)))
                .WithMessage("Contract file must be a PDF or Word document.");
        }
    }
}