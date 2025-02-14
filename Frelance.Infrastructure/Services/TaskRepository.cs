using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class TaskRepository : ITaskRepository
{
    private readonly FrelanceDbContext _context;
    private readonly IUserAccessor _userAccessor;

    public TaskRepository(FrelanceDbContext context, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task AddTaskAsync(CreateTaskCommand createTaskCommand, CancellationToken cancellationToken)
    {
        var taskProject = await _context.Projects.AsNoTracking()
            .Where(x => x.Title == createTaskCommand.CreateProjectTaskRequest.ProjectTitle)
            .FirstOrDefaultAsync(cancellationToken);

        if (taskProject is null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Title)}: '{createTaskCommand.CreateProjectTaskRequest.ProjectTitle}' does not exist");
        }

        var freelancerProfile = await _context.FreelancerProfiles
                                        .AsNoTracking()
                                        .Include(x => x.Users)
                                        .FirstOrDefaultAsync(x => x.Users.UserName == createTaskCommand.CreateProjectTaskRequest.FreelancerUsername, cancellationToken);

        var task = createTaskCommand.Adapt<ProjectTasks>();
        task.ProjectId = taskProject.Id;
        task.Status = ProjectTaskStatus.ToDo.ToString();
        task.Priority = createTaskCommand.CreateProjectTaskRequest.Priority;
        task.Title = createTaskCommand.CreateProjectTaskRequest.Title;
        task.Description = createTaskCommand.CreateProjectTaskRequest.Description;
        task.FreelancerProfileId = freelancerProfile.Id;
        task.CreatedAt = DateTime.UtcNow;
        await _context.Tasks.AddAsync(task, cancellationToken);
    }

    public async Task UpdateTaskAsync(UpdateTaskCommand updateTaskCommand, CancellationToken cancellationToken)
    {
        var projectTaskToUpdate = await _context.Tasks
            .FirstOrDefaultAsync(x => x.Id == updateTaskCommand.Id, cancellationToken);

        if (projectTaskToUpdate is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)}: '{updateTaskCommand.Id}' does not exist");
        }

        projectTaskToUpdate.Title = updateTaskCommand.UpdateProjectTaskRequest.Title;
        projectTaskToUpdate.Description = updateTaskCommand.UpdateProjectTaskRequest.Description;
        projectTaskToUpdate.Status = updateTaskCommand.UpdateProjectTaskRequest.Status;
        projectTaskToUpdate.Priority = updateTaskCommand.UpdateProjectTaskRequest.Priority;
        projectTaskToUpdate.UpdatedAt = DateTime.UtcNow;
        _context.Tasks.Update(projectTaskToUpdate);
    }

    public async Task DeleteTaskAsync(DeleteTaskCommand deleteTaskCommand, CancellationToken cancellationToken)
    {
        var projectTaskToDelete = await _context.Tasks
            .FirstOrDefaultAsync(x => x.Id == deleteTaskCommand.Id, cancellationToken);

        if (projectTaskToDelete is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)}: '{deleteTaskCommand.Id}' does not exist");
        }

        _context.Tasks.Remove(projectTaskToDelete);
    }

    public async Task<TaskDto> GetTaskByIdAsync(GetTaskByIdQuery getTaskByIdQuery, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.AsNoTracking()
            .Include(x => x.Projects)
            .Include(x => x.TimeLogs)
            .FirstOrDefaultAsync(x => x.Id == getTaskByIdQuery.Id, cancellationToken);

        if (task is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)}: '{getTaskByIdQuery.Id}' does not exist");
        }

        return task.Adapt<TaskDto>();
    }

    public async Task<PaginatedList<TaskDto>> GetTasksAsync(GetTasksQuery getTasksQuery, CancellationToken cancellationToken)
    {
        var tasksQuery = _context.Tasks
            .AsNoTracking()
            .Include(x => x.TimeLogs)
            .ProjectToType<TaskDto>();

        var count = await tasksQuery.CountAsync(cancellationToken);
        var items = await tasksQuery
            .Skip((getTasksQuery.PaginationParams.PageNumber - 1) * getTasksQuery.PaginationParams.PageSize)
            .Take(getTasksQuery.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TaskDto>(items, count, getTasksQuery.PaginationParams.PageNumber, getTasksQuery.PaginationParams.PageSize);
    }
}
