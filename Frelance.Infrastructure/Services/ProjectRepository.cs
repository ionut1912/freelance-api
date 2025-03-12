using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class ProjectRepository : IProjectRepository
{
    private readonly IGenericRepository<Projects> _projectRepository;

    public ProjectRepository(IGenericRepository<Projects> projectRepository)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        _projectRepository = projectRepository;
    }

    public async Task CreateProjectAsync(CreateProjectCommand createProjectCommand, CancellationToken cancellationToken)
    {
        var project = createProjectCommand.CreateProjectRequest.Adapt<Projects>();
        await _projectRepository.CreateAsync(project, cancellationToken);
    }


    public async Task UpdateProjectAsync(UpdateProjectCommand updateProjectCommand, CancellationToken cancellationToken)
    {
        var projectToUpdate = await _projectRepository.Query()
            .Where(x => x.Id == updateProjectCommand.Id)
            .Include(x => x.Technologies)
            .FirstOrDefaultAsync(cancellationToken);
        if (projectToUpdate is null)
            throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Id)} : '{updateProjectCommand.Id}' does not exist");

        updateProjectCommand.UpdateProjectRequest.Adapt(projectToUpdate);
        foreach (var projectTechnology in updateProjectCommand.UpdateProjectRequest.Technologies!.Select(technology =>
                     new ProjectTechnologies
                     {
                         Technology = technology,
                         ProjectId = projectToUpdate.Id
                     }))
            projectToUpdate.Technologies.Add(projectTechnology);
        _projectRepository.Update(projectToUpdate);
    }

    public async Task DeleteProjectAsync(DeleteProjectCommand deleteProjectCommand, CancellationToken cancellationToken)
    {
        var projectToDelete = await _projectRepository.Query()
            .Where(x => x.Id == deleteProjectCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (projectToDelete is null)
            throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Id)} : '{deleteProjectCommand.Id}' does not exist");
        _projectRepository.Delete(projectToDelete);
    }

    public async Task<ProjectDto> FindProjectByIdAsync(GetProjectByIdQuery getProjectByIdQuery,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.Query()
            .Where(x => x.Id == getProjectByIdQuery.Id)
            .Include(x => x.Tasks)
            .ThenInclude(x => x.TimeLogs)
            .Include(x => x.Proposals)
            .Include(x => x.Contracts)
            .Include(x => x.Invoices)
            .Include(x => x.Technologies)
            .FirstOrDefaultAsync(cancellationToken);
        if (project is null)
            throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Id)} : '{getProjectByIdQuery.Id}' does not exist");

        return project.Adapt<ProjectDto>();
    }

    public async Task<PaginatedList<ProjectDto>> FindProjectsAsync(GetProjectsQuery getProjectsQuery,
        CancellationToken cancellationToken)
    {
        var projectQuery = _projectRepository.Query()
            .Include(x => x.Tasks)
            .ThenInclude(x => x.TimeLogs)
            .Include(x => x.Invoices)
            .Include(x => x.Contracts)
            .Include(x => x.Proposals)
            .Include(x => x.Technologies)
            .ProjectToType<ProjectDto>();
        var count = await projectQuery.CountAsync(cancellationToken);
        var items = await projectQuery
            .Skip((getProjectsQuery.PaginationParams.PageNumber - 1) * getProjectsQuery.PaginationParams.PageSize)
            .Take(getProjectsQuery.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ProjectDto>(items, count, getProjectsQuery.PaginationParams.PageNumber,
            getProjectsQuery.PaginationParams.PageSize);
    }
}