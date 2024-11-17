
using Frelamce.Contracts.Requests.ProjectTasks;
using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelamce.Contracts.Projects;
using Frelance.API.Frelance.Application.Commands.Projects.CreateProject;
using Frelance.API.Frelance.Application.Commands.Tasks.CreateTask;
using Frelance.API.Frelance.Domain.Entities;
using Frelance.Application.Commands.Projects.UpdateProject;
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
            .Map(dest => dest.ProjectDto, src => src);
        
        TypeAdapterConfig<PaginatedList<TaskDto>,GetTasksResponse>.NewConfig().Map(dest=>dest.Results, src => src);
        TypeAdapterConfig<CreateProjectTaskRequest,CreateTaskCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<UpdateProjectRequest,UpdateProjectCommand>.NewConfig().Map(dest=>dest, src => src);
        TypeAdapterConfig<ProjectTask,GetTaskByIdResponse>.NewConfig().Map(dest=>dest.TaskDto, src => src);
    }
}