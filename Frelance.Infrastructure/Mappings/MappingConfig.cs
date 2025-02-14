
using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Projects;
using Frelance.Contracts.Requests.ProjectTasks;
using Frelance.Contracts.Requests.Skills;
using Frelance.Contracts.Requests.TimeLogs;
using Frelance.Infrastructure.Entities;
using Mapster;

namespace Frelance.Infrastructure.Mappings
{
    public class MappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateProjectCommand, Projects>.NewConfig();

            TypeAdapterConfig<CreateTaskCommand, ProjectTasks>.NewConfig();

            TypeAdapterConfig<CreateTimeLogCommand, TimeLogs>.NewConfig();

            TypeAdapterConfig<CreateUserCommand, Users>.NewConfig();

            TypeAdapterConfig<CreateProjectRequest, CreateProjectCommand>
                .NewConfig()
                .Map(dest => dest, src => src);

            TypeAdapterConfig<UpdateProjectRequest, UpdateProjectCommand>
                .NewConfig()
                .Map(dest => dest, src => src);

            TypeAdapterConfig<Projects, ProjectDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.Deadline, src => src.Deadline)
                .Map(dest => dest.Technologies, src => src.Technologies)
                .Map(dest => dest.Budget, src => src.Budget)
                .Map(dest => dest.Tasks, src => src.Tasks.Adapt<List<TaskDto>>());

            TypeAdapterConfig<CreateProjectTaskRequest, CreateTaskCommand>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<UpdateProjectTaskRequest, UpdateTaskCommand>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<ProjectTasks, TaskDto>
                       .NewConfig()
                       .Map(dest => dest.Id, src => src.Id)
                       .Map(dest => dest.Title, src => src.Title)
                       .Map(dest => dest.Description, src => src.Description)
                       .Map(dest => dest.ProjectTaskStatus, src => src.Status)
                       .Map(dest => dest.Priority, src => src.Priority)
                       .Map(dest => dest.TimeLogs, src => src.TimeLogs.Adapt<List<TimeLogDto>>());

            TypeAdapterConfig<CreateTimeLogRequest, CreateTimeLogCommand>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<UpdateTimeLogRequest, UpdateTimeLogCommand>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<TimeLogs, TimeLogDto>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<List<Skiills>, List<SkillDto>>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<ClientProfileDto, CreateClientProfileCommand>
                       .NewConfig()
                       .Map(src => src, dest => dest);

            TypeAdapterConfig<ClientProfiles, ClientProfileDto>
                       .NewConfig()
                       .Map(dest => dest.Id, src => src.Id)
                       .Map(dest => dest.User, src => new UserProfileDto(
                           src.Users.Id,
                           src.Users.UserName,
                           src.Users.Email,
                           src.Users.PhoneNumber,
                           src.Users.Reviews.Adapt<List<ReviewsDto>>(),
                           src.Users.Proposals.Adapt<List<ProposalsDto>>()))
                       .Map(dest => dest.Address, src => src.Addresses.Adapt<AddressDto>())
                       .Map(dest => dest.Bio, src => src.Bio)
                       .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                       .Map(dest => dest.Contracts, src => src.Contracts.Adapt<List<ContractsDto>>())
                       .Map(dest => dest.Invoices, src => src.Invoices.Adapt<List<InvoicesDto>>());

            TypeAdapterConfig<FreelancerProfiles, CreateFreelancerProfileCommand>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<FreelancerProfiles, FreelancerProfileDto>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<List<Skiills>, List<SkillRequest>>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<FreelancerProfiles, FreelancerProfileDto>
                       .NewConfig()
                       .Map(dest => dest.Id, src => src.Id)
                       .Map(dest => dest.UserProfileDto, src => new UserProfileDto(
                           src.Users.Id,
                           src.Users.UserName,
                           src.Users.Email,
                           src.Users.PhoneNumber,
                           src.Users.Reviews.Adapt<List<ReviewsDto>>(),
                           src.Users.Proposals.Adapt<List<ProposalsDto>>()))
                       .Map(dest => dest.AddressDto, src => src.Addresses.Adapt<AddressDto>())
                       .Map(dest => dest.Bio, src => src.Bio)
                       .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                       .Map(dest => dest.TaskDtos, src => src.Tasks.Adapt<List<TaskDto>>())
                       .Map(dest => dest.SkillDtos, src => src.Skills.Adapt<List<SkillDto>>())
                       .Map(dest => dest.ForeignLanguages, src => src.ForeignLanguages)
                       .Map(dest => dest.IsAvailable, src => src.IsAvailable)
                       .Map(dest => dest.Experience, src => src.Experience)
                       .Map(dest => dest.Rate, src => src.Rate)
                       .Map(dest => dest.Currency, src => src.Currency)
                       .Map(dest => dest.Rating, src => src.Rating)
                       .Map(dest => dest.PortfolioUrl, src => src.PortfolioUrl);

            TypeAdapterConfig<Skiills, SkillDto>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<Entities.Contracts, ContractsDto>
                       .NewConfig()
                       .Map(dest => dest.Id, src => src.Id)
                       .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
                       .Map(dest => dest.StartDate, src => src.StartDate)
                       .Map(dest => dest.EndDate, src => src.EndDate)
                       .Map(dest => dest.Amount, src => src.Amount)
                       .Map(dest => dest.FreelancerName, src => src.Freelancer.Users.UserName)
                       .Map(dest => dest.ClientName, src => src.Client.Users.UserName)
                       .Map(dest => dest.ContractFileUrl, src => src.ContractFileUrl)
                       .Map(dest => dest.Status, src => src.Status);

            TypeAdapterConfig<Invoices, InvoicesDto>
                       .NewConfig()
                       .Map(dest => dest.Id, src => src.Id)
                       .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
                       .Map(dest => dest.ClientName, src => src.Client.Users.UserName)
                       .Map(dest => dest.FreelancerName, src => src.Freelancer.Users.UserName)
                       .Map(dest => dest.Date, src => src.Date)
                       .Map(dest => dest.Amount, src => src.Amount)
                       .Map(dest => dest.InvoiceFileUrl, src => src.InvoiceFileUrl)
                       .Map(dest => dest.Status, src => src.Status);

            TypeAdapterConfig<Proposals, ProposalsDto>
                       .NewConfig()
                       .Map(dest => dest.Id, src => src.Id)
                       .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
                       .Map(dest => dest.Username, src => src.Proposer.UserName)
                       .Map(dest => dest.ProposedBudget, src => src.ProposedBudget)
                       .Map(dest => dest.Status, src => src.Status)
                       .Map(dest => dest.CreatedAt, src => src.CreatedAt);

            TypeAdapterConfig<Reviews, ReviewsDto>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<Addresses, AddressDto>
                       .NewConfig()
                       .Map(dest => dest, src => src);

            TypeAdapterConfig<Users, UserProfileDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Username, src => src.UserName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.Reviews, src => src.Reviews.Adapt<List<ReviewsDto>>())
                .Map(dest => dest.Proposals, src => src.Proposals.Adapt<List<ProposalsDto>>());


        }
    }
}
