using Frelance.Application.Commands.Projects.CreateProject;
using Frelance.Application.Commands.Projects.UpdateProject;
using Frelance.Application.Commands.Tasks.CreateTask;
using Frelance.Application.Commands.Tasks.UpdateTask;
using Frelance.Application.Commands.TimeLogs.CreateTimeLog;
using Frelance.Application.Commands.TimeLogs.UpdateTimeLog;
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

namespace Frelance.Application.Mapings;

public class MapingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<PaginatedList<ProjectDto>, GetProjectsResponse>.NewConfig()
            .Map(dest => dest.Results, src => src);
        TypeAdapterConfig<CreateProjectRequest, CreateProjectCommand>.NewConfig().Map(dest => dest, src => src);
        TypeAdapterConfig<UpdateProjectRequest, UpdateProjectCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<Project, GetProjectByIdResponse>.NewConfig()
            .Map(dest => dest.Project, src => src);
        
        TypeAdapterConfig<PaginatedList<TaskDto>,GetTasksResponse>.NewConfig().Map(dest=>dest.Results, src => src);
        TypeAdapterConfig<CreateProjectTaskRequest,CreateTaskCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<UpdateProjectTaskRequest,UpdateTaskCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<ProjectTask,GetTaskByIdResponse>.NewConfig().Map(dest=>dest.Task, src => src);
        
        TypeAdapterConfig<PaginatedList<TimeLogDto>,GetTimeLogsResponse>.NewConfig().Map(dest=>dest.Results, src => src);
        TypeAdapterConfig<CreateTimeLogRequest,CreateTimeLogCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<UpdateTimeLogRequest,UpdateTimeLogCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<TimeLog,GetTimeLogByIdResponse>.NewConfig().Map(dest=>dest.TimeLog,src => src);
    }
}