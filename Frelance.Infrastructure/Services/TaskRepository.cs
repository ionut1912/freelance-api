using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Application.Mediatr.Queries.Tasks;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Contracts.Responses.Tasks;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class TaskRepository:ITaskRepository
{
    private readonly FrelanceDbContext _context;

    public TaskRepository(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task AddTaskAsync(CreateTaskCommand createTaskCommand, CancellationToken cancellationToken)
    {
        var taskProject= await _context.Projects.AsNoTracking().Where(x=>x.Title==createTaskCommand.ProjectTitle).FirstOrDefaultAsync(cancellationToken);
        if (taskProject is null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Title)} : '{createTaskCommand.ProjectTitle}' does not exist");
        }

        var task = new ProjectTasks
        {
            ProjectId = taskProject.Id,
            Title = createTaskCommand.Title,
            Description = createTaskCommand.Description,
            Status = ProjectTaskStatus.ToDo,
            Priority = createTaskCommand.Priority,
        };
        await _context.Tasks.AddAsync(task, cancellationToken);
    }

    public async Task UpdateTaskAsync(UpdateTaskCommand updateTaskCommand, CancellationToken cancellationToken)
    {
        var projectTaskToUpdate= await _context.Tasks.FirstOrDefaultAsync(x=>x.Id==updateTaskCommand.Id,cancellationToken);
        if (projectTaskToUpdate is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)} : '{updateTaskCommand.Id}' does not exist");
        }
        
        projectTaskToUpdate.Title = updateTaskCommand.Title;
        projectTaskToUpdate.Description = updateTaskCommand.Description;
        projectTaskToUpdate.Status = updateTaskCommand.Status;
        projectTaskToUpdate.Priority = updateTaskCommand.Priority;
        
        _context.Tasks.Update(projectTaskToUpdate);
    }

    public async Task DeleteTaskAsync(DeleteTaskCommand deleteTaskCommand, CancellationToken cancellationToken)
    {
        var projectTaskToDelete = await _context.Tasks.FirstOrDefaultAsync(x=>x.Id==deleteTaskCommand.Id,cancellationToken);
        if (projectTaskToDelete is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)} : '{deleteTaskCommand.Id}' does not exist");
        }
        _context.Tasks.Remove(projectTaskToDelete);
    }

    public async Task<GetTaskByIdResponse> GetTaskByIdAsync(GetTaskByIdQuery getTaskByIdQuery, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.AsNoTracking()
                                        .Include(x=>x.Projects)
                                        .Include(x=>x.TimeLogs)
                                        .FirstOrDefaultAsync(x=>x.Id==getTaskByIdQuery.Id, cancellationToken);
        
        if (task is null)
        {
            throw new NotFoundException($"{nameof(ProjectTasks)} with {nameof(ProjectTasks.Id)} : '{getTaskByIdQuery.Id}' does not exist");
        }
        return task.Adapt<GetTaskByIdResponse>();
    }

    public async Task<PaginatedList<TaskDto>> GetTasksAsync(GetTasksQuery getTasksQuery, CancellationToken cancellationToken)
    {
        var taskQuery = _context.Tasks.AsNoTracking()
                                                        .Include(x=>x.Projects)
                                                        .Include(x=>x.TimeLogs)
                                                        .ProjectToType<TaskDto>().AsQueryable();
        return await CollectionHelper<TaskDto>.ToPaginatedList(taskQuery,getTasksQuery.PaginationParams.PageNumber,getTasksQuery.PaginationParams.PageSize);
    }
}