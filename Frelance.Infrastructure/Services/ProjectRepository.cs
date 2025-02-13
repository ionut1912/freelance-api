
using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class ProjectRepository : IProjectRepository
{
    private readonly FrelanceDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public ProjectRepository(FrelanceDbContext context, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _context = context;
        _userAccessor = userAccessor;
    }
    public async Task AddProjectAsync(CreateProjectCommand createProjectCommand, CancellationToken cancellationToken)
    {
        var project = createProjectCommand.CreateProjectRequest.Adapt<Projects>();
        await _context.Projects.AddAsync(project, cancellationToken);
    }

    public async Task UpdateProjectAsync(UpdateProjectCommand updateProjectCommand, CancellationToken cancellationToken)
    {
        var projectToUpdate = await _context.Projects.FirstOrDefaultAsync(x => x.Id == updateProjectCommand.Id, cancellationToken);
        if (projectToUpdate is null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Id)} : '{updateProjectCommand.Id}' does not exist");
        }
        projectToUpdate.Description = updateProjectCommand.UpdateProjectRequest.Description;
        projectToUpdate.Title = updateProjectCommand.UpdateProjectRequest.Title;
        projectToUpdate.Deadline = updateProjectCommand.UpdateProjectRequest.Deadline;
        projectToUpdate.Technologies = updateProjectCommand.UpdateProjectRequest.Technologies;
        projectToUpdate.Budget = updateProjectCommand.UpdateProjectRequest.Budget;
        _context.Projects.Update(projectToUpdate);
    }

    public async Task DeleteProjectAsync(DeleteProjectCommand deleteProjectCommand, CancellationToken cancellationToken)
    {
        var projectToDelete = await _context.Projects.FirstOrDefaultAsync(x => x.Id == deleteProjectCommand.Id, cancellationToken);
        if (projectToDelete is null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Id)} : '{deleteProjectCommand.Id}' does not exist");
        }
        _context.Projects.Remove(projectToDelete);
    }

    public async Task<ProjectDto> FindProjectByIdAsync(GetProjectByIdQuery getProjectByIdQuery, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
                                            .AsNoTracking()
                                            .Include(x => x.Tasks)
                                            .Include(x => x.Proposals)
                                            .Include(x => x.Contracts)
                                            .Include(x => x.Invoices)
                                            .FirstOrDefaultAsync(x => x.Id == getProjectByIdQuery.Id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Id)} : '{getProjectByIdQuery.Id}' does not exist");
        }

        return project.Adapt<ProjectDto>();
    }

    public async Task<PaginatedList<ProjectDto>> FindProjectsAsync(GetProjectsQuery getProjectsQuery, CancellationToken cancellationToken)
    {
        var projectQuery = _context.Projects
            .AsNoTracking()
            .Include(x => x.Tasks)
            .Include(x => x.Invoices)
            .Include(x => x.Contracts)
            .Include(x => x.Proposals)
            .ProjectToType<ProjectDto>();

        var count = await projectQuery.CountAsync(cancellationToken);
        var items = await projectQuery
            .Skip((getProjectsQuery.PaginationParams.PageNumber - 1) * getProjectsQuery.PaginationParams.PageSize)
            .Take(getProjectsQuery.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ProjectDto>(items, count, getProjectsQuery.PaginationParams.PageNumber, getProjectsQuery.PaginationParams.PageSize);
    }

}