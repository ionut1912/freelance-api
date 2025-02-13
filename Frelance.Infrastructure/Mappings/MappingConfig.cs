
using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Address;
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
                .Map(dest => dest, src => src);

            TypeAdapterConfig<CreateProjectTaskRequest, CreateTaskCommand>
                .NewConfig()
                .Map(dest => dest, src => src);
            TypeAdapterConfig<UpdateProjectTaskRequest, UpdateTaskCommand>
                .NewConfig()
                .Map(dest => dest, src => src);
            TypeAdapterConfig<ProjectTasks, TaskDto>
                .NewConfig()
                .Map(dest => dest, src => src);

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

            TypeAdapterConfig<AddressRequest, Addresses>
                .NewConfig()
                .ConstructUsing(src => new Addresses(src.Country, src.City, src.Street, src.StreetNumber, src.ZipCode));

            TypeAdapterConfig<ClientProfiles, AddClientProfileCommand>
                .NewConfig()
                .Map(dest => dest.Bio, src => src.Bio);
            TypeAdapterConfig<ClientProfileDto, AddClientProfileCommand>
                .NewConfig()
                .Map(src => src, dest => dest);

            TypeAdapterConfig<ClientProfiles, ClientProfileDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.User, src => new UserClientDto(
                    src.Users.Id,
                    src.Users.UserName,
                    src.Users.Email,
                    src.Users.PhoneNumber,
                    src.Users.Reviews.Adapt<List<ReviewsDto>>(),
                    src.Users.Proposals.Adapt<List<ProposalsDto>>(),
                    src.Projects.Adapt<List<ProjectDto>>()))
                .Map(dest => dest.Address, src => src.Addresses.Adapt<AddressDto>())
                .Map(dest => dest.Bio, src => src.Bio)
                .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                .Map(dest => dest.Contracts, src => src.Contracts.Adapt<List<ContractsDto>>())
                .Map(dest => dest.Invoices, src => src.Invoices.Adapt<List<InvoicesDto>>());

            TypeAdapterConfig<FreelancerProfiles, AddFreelancerProfileCommand>
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
                .Map(dest => dest.UserClientDto, src => new UserClientDto(
                    src.Users.Id,
                    src.Users.UserName,
                    src.Users.Email,
                    src.Users.PhoneNumber,
                    src.Users.Reviews.Adapt<List<ReviewsDto>>(),
                    src.Users.Proposals.Adapt<List<ProposalsDto>>(),
                    src.Projects.Adapt<List<ProjectDto>>()))
                .Map(dest => dest.AddressDto, src => src.Addresses.Adapt<AddressDto>())
                .Map(dest => dest.Bio, src => src.Bio)
                .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                .Map(dest => dest.ContractsDto, src => src.Contracts.Adapt<List<ContractsDto>>())
                .Map(dest => dest.InvoicesDto, src => src.Invoices.Adapt<List<InvoicesDto>>())
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
                .Map(dest => dest.ClientId, src => src.ClientId)
                .Map(dest => dest.FreelancerId, src => src.FreelancerId)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.EndDate, src => src.EndDate)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.ContractFileUrl, src => src.ContractFileUrl)
                .Map(dest => dest.Status, src => src.Status);

            TypeAdapterConfig<Invoices, InvoicesDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Project, src => src.Project.Adapt<ProjectDto>())
                .Map(dest => dest.ClientId, src => src.ClientId)
                .Map(dest => dest.FreelancerId, src => src.FreelancerId)
                .Map(dest => dest.Date, src => src.Date)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.InvoiceFileUrl, src => src.InvoiceFileUrl)
                .Map(dest => dest.Status, src => src.Status);
            
            TypeAdapterConfig<Proposals, ProposalsDto>
                .NewConfig()
                .Map(dest=> dest.Id, src => src.Id)
                .Map(dest=> dest.Project, src => src.Project.Adapt<ProjectDto>())
                .Map(dest=>dest.ProposerId, src => src.ProposerId)
                .Map(dest=>dest.ProposedBudget, src => src.ProposedBudget)
                .Map(dest=>dest.Status, src => src.Status)
                .Map(dest=>dest.CreatedAt, src => src.CreatedAt);

            TypeAdapterConfig<Reviews, ReviewsDto>
                .NewConfig()
                .Map(dest => dest, src => src);

        }
    }
}
