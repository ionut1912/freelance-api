namespace Frelance.Contracts.Dtos;

public record ProjectDto(int Id, string Title, string Description, DateTime CreatedAt, DateTime Deadline, List<string> Technologies, decimal Budget, List<TaskDto> Tasks, ClientProfileDto ClientProfileDto, List<ProposalsDto> Proposals, List<ContractsDto> Contracts, List<InvoicesDto> Invoices);