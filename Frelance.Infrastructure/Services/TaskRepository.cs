using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class TaskRepository : ITaskRepository
{
    private readonly IGenericRepository<FreelancerProfiles> _freelancerRepository;
    private readonly IGenericRepository<Projects> _projectsRepository;
    private readonly IGenericRepository<ProjectTasks> _tasksRepository;

    public TaskRepository(IGenericRepository<ProjectTasks> projectTasksRepository,
        IGenericRepository<Projects> projectsRepository,
        IGenericRepository<FreelancerProfiles> freelancerRepository)
    {
        ArgumentNullException.ThrowIfNull(projectTasksRepository, nameof(projectTasksRepository));
        ArgumentNullException.ThrowIfNull(projectsRepository, nameof(projectsRepository));
        ArgumentNullException.ThrowIfNull(freelancerRepository, nameof(freelancerRepository));
        _tasksRepository = projectTasksRepository;
        _projectsRepository = projectsRepository;
        _freelancerRepository = freelancerRepository;
    }

    public async Task AddTaskAsync(CreateTaskCommand createTaskCommand, CancellationToken cancellationToken)
    {
        var taskProject = await _projectsRepository.Query()
            .Where(x => x.Title == createTaskCommand.CreateProjectTaskRequest.ProjectTitle)
            .FirstOrDefaultAsync(cancellationToken);

        if (taskProject is null)
            throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Title)}: '{createTaskCommand.CreateProjectTaskRequest.ProjectTitle}' does not exist");

        var freelancerProfile = await _freelancerRepository.Query()
            .Where(x => x.Users!.UserName == createTaskCommand.CreateProjectTaskRequest.FreelancerUsername)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(cancellationToken);
        if (freelancerProfile is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Users.UserName)} :{createTaskCommand.CreateProjectTaskRequest.FreelancerUsername} does not exist");

        var task = createTaskCommand.CreateProjectTaskRequest.Adapt<ProjectTasks>();
        task.ProjectId = taskProject.Id;
        task.Status = ProjectTaskStatus.ToDo.ToString();
        task.FreelancerProfileId = freelancerProfile.Id;
        await _tasksRepository.AddAsync(task, cancellationToken);
    }

    public async Task UpdateTaskAsync(UpdateTaskCommand updateTaskCommand, CancellationToken cancellationToken)
    {
        var projectTaskToUpdate = await _tasksRepository.Query()
            .Where(x => x.Id == updateTaskCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (projectTaskToUpdate is null)
            throw new NotFoundException(
                $"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)}: '{updateTaskCommand.Id}' does not exist");

        projectTaskToUpdate = updateTaskCommand.UpdateProjectTaskRequest.Adapt<ProjectTasks>();
        _tasksRepository.Update(projectTaskToUpdate);
    }

    public async Task DeleteTaskAsync(DeleteTaskCommand deleteTaskCommand, CancellationToken cancellationToken)
    {
        var projectTaskToDelete = await _tasksRepository.Query()
            .Where(x => x.Id == deleteTaskCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (projectTaskToDelete is null)
            throw new NotFoundException(
                $"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)}: '{deleteTaskCommand.Id}' does not exist");

        _tasksRepository.Delete(projectTaskToDelete);
    }

    public async Task<TaskDto> GetTaskByIdAsync(GetTaskByIdQuery getTaskByIdQuery, CancellationToken cancellationToken)
    {
        var task = await _tasksRepository.Query()
            .Where(x => x.Id == getTaskByIdQuery.Id)
            .Include(x => x.TimeLogs)
            .FirstOrDefaultAsync(cancellationToken);
        if (task is null)
            throw new NotFoundException(
                $"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)}: '{getTaskByIdQuery.Id}' does not exist");

        return task.Adapt<TaskDto>();
    }

    public async Task<PaginatedList<TaskDto>> GetTasksAsync(GetTasksQuery getTasksQuery,
        CancellationToken cancellationToken)
    {
        var tasksQuery = _tasksRepository.Query()
            .Include(x => x.TimeLogs)
            .ProjectToType<TaskDto>();
        var count = await tasksQuery.CountAsync(cancellationToken);
        var items = await tasksQuery
            .Skip((getTasksQuery.PaginationParams.PageNumber - 1) * getTasksQuery.PaginationParams.PageSize)
            .Take(getTasksQuery.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TaskDto>(items, count, getTasksQuery.PaginationParams.PageNumber,
            getTasksQuery.PaginationParams.PageSize);
    }
}