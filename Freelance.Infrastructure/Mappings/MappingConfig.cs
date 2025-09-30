using Freelance.Application.Mediatr.Commands.Contracts;
using Freelance.Application.Mediatr.Commands.Invoices;
using Freelance.Application.Mediatr.Commands.Projects;
using Freelance.Application.Mediatr.Commands.Proposals;
using Freelance.Application.Mediatr.Commands.Reviews;
using Freelance.Application.Mediatr.Commands.Tasks;
using Freelance.Application.Mediatr.Commands.TimeLogs;
using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.ClientProfile;
using Freelance.Contracts.Requests.Contracts;
using Freelance.Contracts.Requests.FreelancerProfiles;
using Freelance.Contracts.Requests.Invoices;
using Freelance.Contracts.Requests.Projects;
using Freelance.Contracts.Requests.ProjectTasks;
using Freelance.Contracts.Requests.Proposals;
using Freelance.Contracts.Requests.Reviews;
using Freelance.Contracts.Requests.Skills;
using Freelance.Contracts.Requests.TimeLogs;
using Freelance.Infrastructure.Entities;
using Mapster;

namespace Freelance.Infrastructure.Mappings;

public abstract class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<RegisterDto, CreateUserCommand>
            .NewConfig()
            .Map(dest => dest.RegisterDto, src => src);
        TypeAdapterConfig<CreateUserCommand, Users>
            .NewConfig()
            .Map(dest => dest.Email, src => src.RegisterDto.Email)
            .Map(dest => dest.UserName, src => src.RegisterDto.Username)
            .Map(dest => dest.PhoneNumber, src => src.RegisterDto.PhoneNumber)
            .AfterMapping((_, dest) => { dest.CreatedAt = DateTime.UtcNow; });
        TypeAdapterConfig<LoginDto, LoginCommand>
            .NewConfig()
            .Map(dest => dest.LoginDto, src => src);
        TypeAdapterConfig<CreateClientProfileRequest, ClientProfiles>
            .NewConfig()
            .Map(dest => dest.Bio, src => src.User.Bio)
            .Map(dest => dest.Image, src => src.User.Image)
            .AfterMapping((src, dest) =>
            {
                dest.CreatedAt = DateTime.UtcNow;
                dest.Addresses = new Addresses
                {
                    Country = src.Address.AddressCountry,
                    City = src.Address.AddressCity,
                    Street = src.Address.AddressStreet,
                    StreetNumber = src.Address.AddressStreetNumber,
                    ZipCode = src.Address.AddressZip
                };
                dest.IsVerified = false;
            });
        TypeAdapterConfig<ClientProfiles, ClientProfileDto>
            .NewConfig()
            .ConstructUsing(src => new ClientProfileDto(
                src.Id,
                src.Users.Adapt<UserProfileDto>(),
                src.Addresses.Adapt<AddressDto>(),
                src.Bio,
                src.Projects.Adapt<List<ProjectDto>>(),
                src.Contracts.Adapt<List<ContractsDto>>(),
                src.Invoices.Adapt<List<InvoicesDto>>(),
                src.Image,
                src.IsVerified));
        TypeAdapterConfig<Users, UserProfileDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Username, src => src.UserName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.Reviews, src => src.Reviews.Adapt<ReviewsDto>())
            .Map(dest => dest.Proposals, src => src.Proposals.Adapt<ProposalsDto>())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);
        TypeAdapterConfig<Addresses, AddressDto>
            .NewConfig()
            .Map(dest => dest, src => src);
        TypeAdapterConfig<Entities.Contracts, ContractsDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
            .Map(dest => dest.ClientName, src => src.Client.Users!.UserName)
            .Map(dest => dest.FreelancerName, src => src.Freelancer.Users!.UserName)
            .Map(dest => dest.StartDate, src => src.StartDate)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.ContractFile, src => src.ContractFile);
        TypeAdapterConfig<Invoices, InvoicesDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
            .Map(dest => dest.ClientName, src => src.Client!.Users!.UserName)
            .Map(dest => dest.FreelancerName, src => src.Freelancer!.Users!.UserName)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.InvoiceFile, src => src.InvoiceFile);
        TypeAdapterConfig<Reviews, ReviewsDto>
            .NewConfig()
            .Map(dest => dest, src => src);
        TypeAdapterConfig<Proposals, ProposalsDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
            .Map(dest => dest.Username, src => src.Proposer!.UserName)
            .Map(dest => dest.ProposedBudget, src => src.ProposedBudget)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
        TypeAdapterConfig<Projects, ProjectDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CreatedAt.Date, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.Deadline, src => src.Deadline)
            .Map(dest => dest.Technologies, src => src.Technologies.Adapt<List<ProjectTechnologiesDto>>())
            .Map(dest => dest.Budget, src => src.Budget)
            .Map(dest => dest.Tasks, src => src.Tasks.Adapt<List<TaskDto>>());
        TypeAdapterConfig<ProjectTechnologies, ProjectTechnologiesDto>
            .NewConfig()
            .Map(dest => dest, src => src);
        TypeAdapterConfig<ProjectTasks, TaskDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.ProjectTaskStatus, src => src.Status)
            .Map(dest => dest.Priority, src => src.Priority)
            .Map(dest => dest.TimeLogs, src => src.TimeLogs.Adapt<List<TimeLogDto>>())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
        TypeAdapterConfig<TimeLogs, TimeLogDto>
            .NewConfig()
            .Map(dest => dest, src => src);
        TypeAdapterConfig<CreateContractRequest, CreateContractCommand>
            .NewConfig()
            .Map(dest => dest.CreateContractRequest, src => src);
        TypeAdapterConfig<CreateContractRequest, Entities.Contracts>
            .NewConfig()
            .Map(dest => dest.StartDate, src => src.StartDate)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.ContractFile, src => src.ContractFile)
            .AfterMapping((_, dest) => { dest.CreatedAt = DateTime.UtcNow; });
        TypeAdapterConfig<UpdateContractRequest, UpdateContractCommand>
            .NewConfig()
            .Map(dest => dest.UpdateContractRequest, src => src);
        TypeAdapterConfig<UpdateContractRequest, Entities.Contracts>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.EndDate = src.EndDate ?? dest.EndDate;
                dest.Amount = src.Amount ?? dest.Amount;
                dest.ContractFile = src.ContractFile ?? dest.ContractFile;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<string, FreelancerForeignLanguage>
            .NewConfig()
            .Map(dest => dest.Language, src => src);
        TypeAdapterConfig<CreateFreelancerProfileRequest, FreelancerProfiles>
            .NewConfig()
            .Map(dest => dest.Bio, src => src.User.Bio)
            .Map(dest => dest.Experience, src => src.Freelancer.Experience)
            .Map(dest => dest.Rate, src => src.Freelancer.Rate)
            .Map(dest => dest.Currency, src => src.Freelancer.Currency)
            .Map(dest => dest.PortfolioUrl, src => src.Freelancer.PortfolioUrl)
            .Map(dest => dest.Image, src => src.User.Image)
            .Ignore(dest => dest.FreelancerProfileSkills)
            .Ignore(dest => dest.ForeignLanguages)
            .AfterMapping((src, dest) =>
            {
                dest.Addresses = new Addresses
                {
                    Country = src.Address.AddressCountry,
                    Street = src.Address.AddressStreet,
                    StreetNumber = src.Address.AddressStreetNumber,
                    City = src.Address.AddressCity,
                    ZipCode = src.Address.AddressZip
                };
                dest.ForeignLanguages = src.Freelancer.ForeignLanguages
                    .Select(lang => new FreelancerForeignLanguage { Language = lang,CreatedAt = DateTime.UtcNow})
                    .ToList();
                dest.IsVerified = false;
                dest.CreatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<List<SkillRequest>, List<Skills>>
            .NewConfig()
            .Map(src => src, dest => dest);
        TypeAdapterConfig<CreateInvoiceRequest, CreateInvoiceCommand>
            .NewConfig()
            .Map(dest => dest.CreateInvoiceRequest, src => src);
        TypeAdapterConfig<CreateInvoiceRequest, Invoices>
            .NewConfig()
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.InvoiceFile, src => src.InvoiceFile)
            .AfterMapping((_, dest) => { dest.CreatedAt = DateTime.UtcNow; });
        TypeAdapterConfig<UpdateInvoiceRequest, UpdateInvoiceCommand>
            .NewConfig()
            .Map(dest => dest.UpdateInvoiceRequest, src => src);
        TypeAdapterConfig<UpdateInvoiceRequest, Invoices>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.Status = src.Status ?? dest.Status;
                dest.Amount = src.Amount ?? dest.Amount;
                dest.InvoiceFile = src.InvoiceFile ?? dest.InvoiceFile;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<CreateProjectRequest, CreateProjectCommand>
            .NewConfig()
            .Map(dest => dest.CreateProjectRequest, src => src);
        TypeAdapterConfig<CreateProjectRequest, Projects>
            .NewConfig()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Deadline, src => src.Deadline)
            .Map(dest => dest.Budget, src => src.Budget)
            .Ignore(dest => dest.Technologies)
            .AfterMapping((src, dest) =>
            {
                dest.CreatedAt = DateTime.UtcNow;
                dest.Technologies = src.Technologies.Select(tech => new ProjectTechnologies { Technology = tech })
                    .ToList();
                dest.CreatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<UpdateProjectRequest, UpdateProjectCommand>
            .NewConfig()
            .Map(dest => dest.UpdateProjectRequest, src => src);
        TypeAdapterConfig<UpdateProjectRequest, Projects>
            .NewConfig()
            .Ignore(dest => dest.Technologies)
            .AfterMapping((src, dest) =>
            {
                dest.Title = src.Title ?? dest.Title;
                dest.Description = src.Description ?? dest.Description;
                dest.Deadline = src.Deadline ?? dest.Deadline;
                dest.Budget = src.Budget ?? dest.Budget;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<CreateProposalRequest, CreateProposalCommand>
            .NewConfig()
            .Map(dest => dest.CreateProposalRequest, src => src);
        TypeAdapterConfig<CreateProposalRequest, Proposals>
            .NewConfig()
            .Map(dest => dest.ProposedBudget, src => src.ProposedBudget)
            .AfterMapping((_, dest) => { dest.CreatedAt = DateTime.UtcNow; });
        TypeAdapterConfig<UpdateProposalCommand, UpdateProposalRequest>
            .NewConfig()
            .Map(dest => dest, src => src.UpdateProposalRequest);
        TypeAdapterConfig<UpdateProposalRequest, Proposals>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.ProposedBudget = src.ProposedBudget;
                dest.Status = src.Status ?? dest.Status;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<CreateReviewRequest, CreateReviewCommand>
            .NewConfig()
            .Map(dest => dest.CreateReviewRequest, src => src);
        TypeAdapterConfig<CreateReviewRequest, Reviews>
            .NewConfig()
            .Map(dest => dest.ReviewText, src => src.ReviewText)
            .AfterMapping((_, dest) => { dest.CreatedAt = DateTime.UtcNow; });
        TypeAdapterConfig<UpdateReviewRequest, UpdateReviewCommand>
            .NewConfig()
            .Map(dest => dest.UpdateReviewRequest, src => src);
        TypeAdapterConfig<UpdateReviewRequest, Reviews>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.ReviewText = src.ReviewText ?? dest.ReviewText;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<CreateTimeLogRequest, CreateTimeLogCommand>
            .NewConfig()
            .Map(dest => dest.CreateTimeLogRequest, src => src);
        TypeAdapterConfig<CreateTimeLogRequest, TimeLogs>
            .NewConfig()
            .Map(dest => dest.StartTime, src => src.StartTime)
            .Map(dest => dest.EndTime, src => src.EndTime)
            .AfterMapping((_, dest) => { dest.CreatedAt = DateTime.UtcNow; });
        TypeAdapterConfig<UpdateTimeLogRequest, UpdateTimeLogCommand>
            .NewConfig()
            .Map(dest => dest.UpdateTimeLogRequest, src => src);
        TypeAdapterConfig<UpdateTimeLogRequest, TimeLogs>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.StartTime = src.StartTime ?? dest.StartTime;
                dest.EndTime = src.EndTime ?? dest.EndTime;
                dest.TotalHours = src.TotalHours ?? dest.TotalHours;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<CreateProjectTaskRequest, CreateTaskCommand>
            .NewConfig()
            .Map(dest => dest.CreateProjectTaskRequest, src => src);
        TypeAdapterConfig<CreateProjectTaskRequest, ProjectTasks>
            .NewConfig()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Priority, src => src.Priority)
            .AfterMapping((_, dest) => { dest.CreatedAt = DateTime.UtcNow; });
        TypeAdapterConfig<UpdateProjectTaskRequest, UpdateTaskCommand>
            .NewConfig()
            .Map(dest => dest.UpdateProjectTaskRequest, src => src);
        TypeAdapterConfig<UpdateProjectTaskRequest, ProjectTasks>
            .NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.Title = src.Title ?? dest.Title;
                dest.Description = src.Description ?? dest.Description;
                dest.Priority = src.Priority ?? dest.Priority;
                dest.Status = src.Status ?? dest.Status;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        TypeAdapterConfig<FreelancerProfiles, FreelancerProfileDto>
            .NewConfig()
            .ConstructUsing(src => new FreelancerProfileDto(
                src.Id,
                src.Users.Adapt<UserProfileDto>(),
                src.Addresses.Adapt<AddressDto>(),
                src.Bio,
                src.Projects.Adapt<List<ProjectDto>>(),
                src.Contracts.Adapt<List<ContractsDto>>(),
                src.Invoices.Adapt<List<InvoicesDto>>(),
                src.Image,
                src.IsVerified,
                src.Tasks.Adapt<List<TaskDto>>(),
                src.FreelancerProfileSkills.Select(fps => fps.Skill).Adapt<List<SkillDto>>(),
                src.ForeignLanguages.Adapt<List<ForeignLanguageDto>>(),
                src.Experience,
                src.Rate,
                src.Currency,
                src.Rating ?? 0,
                src.PortfolioUrl));
        TypeAdapterConfig<List<Skills>, List<SkillDto>>
            .NewConfig()
            .Map(dest => dest, src => src);
        TypeAdapterConfig<List<FreelancerForeignLanguage>, List<ForeignLanguageDto>>
            .NewConfig()
            .Map(dest => dest, src => src);
    }
}