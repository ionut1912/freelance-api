using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelance.Contracts.Enums;
using Frelance.API.Frelance.Domain.Entities;
using Frelance.API.Frelance.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.API.Frelance.Application.Commands.Tasks.CreateTask;

public class CreateTaskCommandHandler:IRequestHandler<CreateTaskCommand,int>
{
    private readonly FrelanceDbContext _context;

    public CreateTaskCommandHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var taskProject= await _context.Projects.AsNoTracking().Where(x=>x.Title==request.ProjectTitle).FirstOrDefaultAsync(cancellationToken);
        if (taskProject is null)
        {
            throw new NotFoundException($"{nameof(Project)} with {nameof(Project.Title)} : '{request.ProjectTitle}' does not exist");
        }

        var task = new ProjectTask
        {
            ProjectId = taskProject.Id,
            Title = request.Title,
            Description = request.Description,
            Status = Status.ToDo,
            Priority = request.Priority,
        };
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return task.Id;
    }
}