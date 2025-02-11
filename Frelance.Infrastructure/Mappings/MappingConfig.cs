using System.Collections.Generic;
using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Projects;
using Frelance.Contracts.Requests.ProjectTasks;
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

            TypeAdapterConfig<Addresses, AddressDto>
                .NewConfig()
                .Map(dest => dest, src => src);
            TypeAdapterConfig<ClientProfiles, AddClientProfileCommand>
                .NewConfig()
                .Map(dest => dest.Address, src => src.Addresses)
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
                    src.Users.Proposals.Adapt<List<ProposalsDto>>()))
                .Map(dest => dest.Address, src => src.Addresses.Adapt<AddressDto>())
                .Map(dest => dest.Bio, src => src.Bio)
                .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                .Map(dest => dest.Contracts, src => src.Contracts.Adapt<List<ContractsDto>>())
                .Map(dest => dest.Invoices, src => src.Invoices.Adapt<List<InvoicesDto>>());


        }
    }
}
