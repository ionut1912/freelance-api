using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Commands.Contracts;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Commands.Invoices;
using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Commands.Proposals;
using Frelance.Application.Mediatr.Commands.Reviews;
using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Contracts;
using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Contracts.Requests.Invoices;
using Frelance.Contracts.Requests.Projects;
using Frelance.Contracts.Requests.ProjectTasks;
using Frelance.Contracts.Requests.Proposals;
using Frelance.Contracts.Requests.Reviews;
using Frelance.Contracts.Requests.Skills;
using Frelance.Contracts.Requests.TimeLogs;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Frelance.Infrastructure.Mappings
{
    public class MappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<IFormFile, IFormFile>
                .NewConfig()
                .Ignore(dest => dest.Headers);

            TypeAdapterConfig<IFormFile, IFormFile>
                .NewConfig()
                .MapWith(src => src);


            TypeAdapterConfig<RegisterDto, CreateUserCommand>
                .NewConfig()
                .Map(dest => dest.RegisterDto, src => src);

            TypeAdapterConfig<CreateUserCommand, Users>
                .NewConfig()
                .Map(dest => dest.Email, src => src.RegisterDto.Email)
                .Map(dest => dest.UserName, src => src.RegisterDto.Username)
                .Map(dest => dest.PhoneNumber, src => src.RegisterDto.PhoneNumber)
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<LoginDto, LoginCommand>
                .NewConfig()
                .Map(dest => dest.LoginDto, src => src);

            TypeAdapterConfig<CreateClientProfileRequest, CreateClientProfileCommand>
                .NewConfig()
                .Map(dest => dest.CreateClientProfileRequest, src => src);

            TypeAdapterConfig<CreateClientProfileCommand, ClientProfiles>
                .NewConfig()
                .Map(dest => dest.Addresses, src =>
                    new Addresses
                    {
                        Country = src.CreateClientProfileRequest.AddressCountry,
                        City = src.CreateClientProfileRequest.AddressCity,
                        Street = src.CreateClientProfileRequest.AddressStreet,
                        StreetNumber = src.CreateClientProfileRequest.AddressStreetNumber,
                        ZipCode = src.CreateClientProfileRequest.AddressZip
                    })
                .Map(dest => dest.Bio, src => src.CreateClientProfileRequest.Bio)
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<ClientProfiles, ClientProfileDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.User, src => src.Users.Adapt<UserProfileDto>())
                .Map(dest => dest.Address, src => src.Addresses.Adapt<AddressDto>())
                .Map(dest => dest.Bio, src => src.Bio)
                .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                .Map(dest => dest.Contracts, src => src.Contracts.Adapt<List<ContractsDto>>())
                .Map(dest => dest.Invoices, src => src.Invoices.Adapt<List<InvoicesDto>>())
                .Map(dest => dest.Projects, src => src.Projects.Adapt<List<ProjectDto>>());

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
                .Map(dest => dest.ContractFileUrl, src => src.ContractFileUrl)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);

            TypeAdapterConfig<Invoices, InvoicesDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
                .Map(dest => dest.ClientName, src => src.Client!.Users!.UserName)
                .Map(dest => dest.FreelancerName, src => src.Freelancer!.Users!.UserName)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.InvoiceFileUrl, src => src.InvoiceFileUrl)
                .Map(dest => dest.Status, src => src.Status);

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
                .Map(dest => dest.Technologies, src => src.Technologies.Adapt<ProjectTechnologiesDto>())
                .Map(dest => dest.Budget, src => src.Budget)
                .Map(dest => dest.Tasks, src => src.Tasks.Adapt<TaskDto>());

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
                .Map(dest => dest.TimeLogs, src => src.TimeLogs.Adapt<TimeLogDto>())
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);

            TypeAdapterConfig<TimeLogs, TimeLogDto>
                .NewConfig()
                .Map(dest => dest, src => src);

            TypeAdapterConfig<UpdateClientProfileRequest, UpdateClientProfileCommand>
                .NewConfig()
                .Map(dest => dest.UpdateClientProfileRequest, src => src);

            TypeAdapterConfig<UpdateClientProfileCommand, ClientProfiles>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .AfterMapping((src, dest) =>
                {
                    var req = src.UpdateClientProfileRequest;
                    dest.Addresses!.Country = !string.IsNullOrEmpty(req.AddressCountry) ? req.AddressCountry : dest.Addresses.Country;
                    dest.Addresses.Street = !string.IsNullOrEmpty(req.AddressStreet) ? req.AddressStreet : dest.Addresses.Street;
                    dest.Addresses.StreetNumber = !string.IsNullOrEmpty(req.AddressStreetNumber) ? req.AddressStreetNumber : dest.Addresses.StreetNumber;
                    dest.Addresses.City = !string.IsNullOrEmpty(req.AddressCity) ? req.AddressCity : dest.Addresses.City;
                    dest.Addresses.ZipCode = !string.IsNullOrEmpty(req.AddressZip) ? req.AddressZip : dest.Addresses.ZipCode;
                    dest.Bio = !string.IsNullOrEmpty(req.Bio) ? req.Bio : dest.Bio;
                    dest.CreatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<CreateContractRequest, CreateContractCommand>
                .NewConfig()
                .Map(dest => dest.CreateContractRequest, src => src);

            TypeAdapterConfig<CreateContractCommand, Entities.Contracts>
                .NewConfig()
                .Map(dest => dest.StartDate, src => src.CreateContractRequest.StartDate)
                .Map(dest => dest.EndDate, src => src.CreateContractRequest.EndDate)
                .Map(dest => dest.Amount, src => src.CreateContractRequest.Amount)
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<UpdateContractRequest, UpdateContractCommand>
                .NewConfig()
                .Map(dest => dest.UpdateContractRequest, src => src);

            TypeAdapterConfig<Entities.Contracts, UpdateContractCommand>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .AfterMapping((src, dest) =>
                {
                    var req = dest.UpdateContractRequest;
                    src.EndDate = req.EndDate ?? src.EndDate;
                    src.Amount = req.Amount ?? src.Amount;
                    src.Status = req.Status ?? src.Status;
                    src.UpdatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<CreateFreelancerProfileRequest, CreateFreelancerProfileCommand>
                .NewConfig()
                .Map(dest => dest.CreateFreelancerProfileRequest, src => src);

            TypeAdapterConfig<string, FreelancerForeignLanguage>
                .NewConfig()
                .Map(dest => dest.Language, src => src);

            TypeAdapterConfig<CreateFreelancerProfileRequest, FreelancerProfiles>
                .NewConfig()
                .Map(dest => dest.Bio, src => src.Bio)
                .Map(dest => dest.Experience, src => src.Experience)
                .Map(dest => dest.Rate, src => src.Rate)
                .Map(dest => dest.Currency, src => src.Currency)
                .Map(dest => dest.Rating, src => src.Rating)
                .Map(dest => dest.PortfolioUrl, src => src.PortfolioUrl)
                .Ignore(dest => dest.Skills)
                .Ignore(dest => dest.ForeignLanguages) // Prevent automatic mapping of foreign languages.
                .AfterMapping((src, dest) =>
                {
                    dest.Addresses = new Addresses
                    {
                        Country = src.AddressCountry,
                        Street = src.AddressStreet,
                        StreetNumber = src.AddressStreetNumber,
                        City = src.AddressCity,
                        ZipCode = src.AddressZip
                    };

                    if (src.ProgrammingLanguages != null && src.Areas != null)
                    {
                        var count = Math.Min(src.ProgrammingLanguages.Count, src.Areas.Count);
                        for (var i = 0; i < count; i++)
                        {
                            dest.Skills.Add(new Skills
                            {
                                ProgrammingLanguage = src.ProgrammingLanguages[i],
                                Area = src.Areas[i]
                            });
                        }
                    }

                    if (src.ForeignLanguages != null)
                    {
                        dest.ForeignLanguages = src.ForeignLanguages
                            .Select(lang => new FreelancerForeignLanguage { Language = lang })
                            .ToList();
                    }
                    dest.CreatedAt = DateTime.UtcNow;
                });



            TypeAdapterConfig<List<SkillRequest>, List<Skills>>
                .NewConfig()
                .Map(src => src, dest => dest);
            TypeAdapterConfig<UpdateFreelancerProfileCommand, UpdateFreelancerProfileRequest>
                .NewConfig()
                .Map(dest => dest, src => src.UpdateFreelancerProfileRequest);

            TypeAdapterConfig<UpdateFreelancerProfileRequest, FreelancerProfiles>
                .NewConfig()
                .AfterMapping((src, dest) =>
                {
                    dest.Addresses!.Country = src.AddressCountry ?? dest.Addresses.Country;
                    dest.Addresses.Street = src.AddressStreet ?? dest.Addresses.Street;
                    dest.Addresses.StreetNumber = src.AddressStreetNumber ?? dest.Addresses.StreetNumber;
                    dest.Addresses.City = src.AddressCity ?? dest.Addresses.City;
                    dest.Addresses.ZipCode = src.AddressZip ?? dest.Addresses.ZipCode;
                    dest.Bio = src.Bio ?? dest.Bio;
                    if (src.ProgrammingLanguages != null && src.Areas != null)
                    {
                        dest.Skills = [];
                        var count = Math.Min(src.ProgrammingLanguages.Count, src.Areas.Count);
                        for (var i = 0; i < count; i++)
                        {
                            dest.Skills.Add(new Skills
                            {
                                ProgrammingLanguage = src.ProgrammingLanguages[i],
                                Area = src.Areas[i]
                            });
                        }
                    }
                    if (src.ForeignLanguages != null)
                    {
                        dest.ForeignLanguages = src.ForeignLanguages
                            .Select(lang => new FreelancerForeignLanguage { Language = lang })
                            .ToList();
                    }
                    dest.Experience = src.Experience ?? dest.Experience;
                    dest.Rate = src.Rate;
                    dest.Currency = src.Currency ?? dest.Currency;
                    dest.Rating = src.Rating;
                    dest.PortfolioUrl = src.PortfolioUrl ?? dest.PortfolioUrl;
                    dest.UpdatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<CreateInvoiceRequest, CreateInvoiceCommand>
                .NewConfig()
                .Map(dest => dest.CreateInvoiceRequest, src => src);

            TypeAdapterConfig<CreateInvoiceCommand, Invoices>
                .NewConfig()
                .Map(dest => dest.Amount, src => src.CreateInvoiceRequest.Amount)
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<UpdateInvoiceRequest, UpdateInvoiceCommand>
                .NewConfig()
                .Map(dest => dest.UpdateInvoiceRequest, src => src);

            TypeAdapterConfig<UpdateInvoiceRequest, Invoices>
                .NewConfig()
                .AfterMapping((src, dest) =>
                {
                    dest.Status = src.Status ?? dest.Status;
                    dest.Amount = src.Amount ?? dest.Amount;
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
                    dest.Technologies = src.Technologies
                        .Select(tech => new ProjectTechnologies { Technology = tech })
                        .ToList();
                    dest.CreatedAt = DateTime.UtcNow;
                });


            TypeAdapterConfig<UpdateProjectRequest, UpdateProjectCommand>
                .NewConfig()
                .Map(dest => dest.UpdateProjectRequest, src => src);

            TypeAdapterConfig<UpdateProjectRequest, Projects>
                .NewConfig()
                .AfterMapping((src, dest) =>
                {
                    dest.Title = src.Title ?? dest.Title;
                    dest.Description = src.Description ?? dest.Description;
                    dest.Deadline = src.Deadline ?? dest.Deadline;
                    dest.Budget = src.Budget ?? dest.Budget;
                    if (src.Technologies is not null)
                    {
                        dest.Technologies = src.Technologies
                            .Select(tech => new ProjectTechnologies { Technology = tech })
                            .ToList();
                    }

                    dest.Technologies = dest.Technologies;
                    dest.UpdatedAt = DateTime.UtcNow;
                });

            TypeAdapterConfig<CreateProposalRequest, CreateProposalCommand>
                .NewConfig()
                .Map(dest => dest.CreateProposalRequest, src => src);

            TypeAdapterConfig<CreateProposalRequest, Proposals>
                .NewConfig()
                .Map(dest => dest.ProposedBudget, src => src.ProposedBudget)
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

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
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

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
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

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
                .AfterMapping((src, dest) =>
                {
                    dest.CreatedAt = DateTime.UtcNow;
                });

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
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UserProfile, src => src.Users.Adapt<UserProfileDto>())
                .Map(dest => dest.Address, src => src.Addresses.Adapt<AddressDto>())
                .Map(dest => dest.Bio, src => src.Bio)
                .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                .Map(dest => dest.Tasks, src => src.Tasks.Adapt<List<TaskDto>>())
                .Map(dest => dest.Skills, src => src.Skills.Adapt<List<SkillDto>>())
                .Map(dest => dest.ForeignLanguages, src => src.ForeignLanguages.Adapt<List<ForeignLanguageDto>>())
                .Map(dest => dest.Projects, src => src.Projects.Adapt<List<ProjectDto>>())
                .Map(dest => dest.IsAvailable, src => src.IsAvailable)
                .Map(dest => dest.Experience, src => src.Experience)
                .Map(dest => dest.Rate, src => src.Rate)
                .Map(dest => dest.Currency, src => src.Currency)
                .Map(dest => dest.Rating, src => src.Rating)
                .Map(dest => dest.PortfolioUrl, src => src.PortfolioUrl);

            TypeAdapterConfig<List<Skills>, List<SkillDto>>
                .NewConfig()
                .Map(dest => dest, src => src);

            TypeAdapterConfig<List<FreelancerForeignLanguage>, List<ForeignLanguageDto>>
                .NewConfig()
                .Map(dest => dest, src => src);


        }

    }
}
