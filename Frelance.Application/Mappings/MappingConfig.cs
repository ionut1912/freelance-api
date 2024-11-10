using Frelance.API.Frelance.Contracts.Dtos;
using Frelance.API.Frelance.Contracts.Responses;
using Frelance.Domain.Entities;
using Mapster;

namespace Frelance.Application.Mappings;

public class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<PaginatedList<ProjectDto>, GetProjectsResponse>.NewConfig()
            .Map(dest => dest.Results, src => src);
        
        TypeAdapterConfig<Project, GetProjectByIdResponse>.NewConfig()
            .Map(dest => dest.ProjectDto, src => src);
    }
}