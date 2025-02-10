
using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Responses.Projects;
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
        var project = createProjectCommand.Adapt<Projects>();
        var freelancerProfile = await _context.FreelancerProfiles
                                                .AsNoTracking()
                                                .Include(x => x.Users)
                                                .FirstOrDefaultAsync(x => x.Users.UserName == _userAccessor.GetUsername(), cancellationToken);

        project.FreelancerProfiles = freelancerProfile;
        await _context.Projects.AddAsync(project, cancellationToken);
    }

    public async Task UpdateProjectAsync(UpdateProjectCommand updateProjectCommand, CancellationToken cancellationToken)
    {
        var projectToUpdate = await _context.Projects.FirstOrDefaultAsync(x => x.Id == updateProjectCommand.Id, cancellationToken);
        if (projectToUpdate is null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Id)} : '{updateProjectCommand.Id}' does not exist");
        }
        projectToUpdate.Description = updateProjectCommand.Description;
        projectToUpdate.Title = updateProjectCommand.Title;
        projectToUpdate.Deadline = updateProjectCommand.Deadline;
        projectToUpdate.Technologies = updateProjectCommand.Technologies;
        projectToUpdate.Budget = updateProjectCommand.Budget;
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

    public async Task<GetProjectByIdResponse> FindProjectByIdAsync(GetProjectByIdQuery getProjectByIdQuery, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.AsNoTracking().Include(x => x.Tasks).FirstOrDefaultAsync(x => x.Id == getProjectByIdQuery.Id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Id)} : '{getProjectByIdQuery.Id}' does not exist");
        }

        return project.Adapt<GetProjectByIdResponse>();
    }

    public async Task<PaginatedList<ProjectDto>> FindProjectsAsync(GetProjectsQuery getProjectsQuery, CancellationToken cancellationToken)
    {
        var projectQuery = _context.Projects.ProjectToType<ProjectDto>().AsQueryable();
        return await CollectionHelper<ProjectDto>.ToPaginatedList(projectQuery, getProjectsQuery.PaginationParams.PageNumber, getProjectsQuery.PaginationParams.PageSize);
    }
}