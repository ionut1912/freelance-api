using Freelance.Application.Mediatr.Commands.Projects;
using Freelance.Application.Mediatr.Queries.Projects;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Exceptions;
using Freelance.Contracts.Responses.Common;
using Freelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Freelance.Infrastructure.Services;

public class ProjectRepository : IProjectRepository
{
    private readonly IGenericRepository<Projects> _projectRepository;
    private readonly IGenericRepository<ClientProfiles> _clientProfilesRepository;
    private readonly IUserAccessor  _userAccessor;
    public ProjectRepository(IGenericRepository<Projects> projectRepository, IGenericRepository<ClientProfiles> clientProfilesRepository,IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        ArgumentNullException.ThrowIfNull(clientProfilesRepository, nameof(clientProfilesRepository));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _projectRepository = projectRepository;
        _clientProfilesRepository = clientProfilesRepository;
        _userAccessor = userAccessor;
    }

    public async Task CreateProjectAsync(CreateProjectCommand createProjectCommand, CancellationToken cancellationToken)
    {
        var project = createProjectCommand.CreateProjectRequest.Adapt<Projects>();
        var clientProfile = await _clientProfilesRepository.Query()
            .Include(x => x.Users)
            .Where(x => x.Users!.UserName == _userAccessor.GetUsername())
            .FirstOrDefaultAsync(cancellationToken)??throw new NotFoundException($"Profile for user with username ${_userAccessor.GetUsername()} not found");
        clientProfile.Projects!.Add(project);
        foreach (var technology in project.Technologies)
        {
            technology.CreatedAt=DateTime.UtcNow;
        }
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
        return project is null
            ? throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Id)} : '{getProjectByIdQuery.Id}' does not exist")
            : project.Adapt<ProjectDto>();
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