using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Projects;
using Frelance.Contracts.Requests.ProjectTasks;
using Frelance.Contracts.Requests.TimeLogs;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Responses.Projects;
using Frelance.Contracts.Responses.Tasks;
using Frelance.Contracts.Responses.TimeLogs;
using Frelance.Infrastructure.Entities;
using Mapster;

namespace Frelance.Infrastructure.Mappings;


public class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<CreateProjectCommand, Projects>.NewConfig();
        TypeAdapterConfig<CreateTaskCommand,ProjectTasks>.NewConfig();
        TypeAdapterConfig<CreateTimeLogCommand, TimeLogs>.NewConfig();
        TypeAdapterConfig<CreateUserCommand, Users>.NewConfig();
        
        TypeAdapterConfig<PaginatedList<ProjectDto>, GetProjectsResponse>.NewConfig()
            .Map(dest => dest.Results, src => src);
        TypeAdapterConfig<CreateProjectRequest, CreateProjectCommand>.NewConfig().Map(dest => dest, src => src);
        TypeAdapterConfig<UpdateProjectRequest, UpdateProjectCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<Projects, GetProjectByIdResponse>.NewConfig()
            .Map(dest => dest.Project, src => src);
        
        TypeAdapterConfig<PaginatedList<TaskDto>,GetTasksResponse>.NewConfig().Map(dest=>dest.Results, src => src);
        TypeAdapterConfig<CreateProjectTaskRequest,CreateTaskCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<UpdateProjectTaskRequest,UpdateTaskCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<ProjectTasks,GetTaskByIdResponse>.NewConfig().Map(dest=>dest.Task, src => src);
        
        TypeAdapterConfig<PaginatedList<TimeLogDto>,GetTimeLogsResponse>.NewConfig().Map(dest=>dest.Results, src => src);
        TypeAdapterConfig<CreateTimeLogRequest,CreateTimeLogCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<UpdateTimeLogRequest,UpdateTimeLogCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<TimeLogs,GetTimeLogByIdResponse>.NewConfig().Map(dest=>dest.TimeLog,src => src);
    }
}