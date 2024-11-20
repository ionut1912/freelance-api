using Frelance.Application.Mediatr.Commands.Tasks;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Exceptions;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Mediatr.Handlers.Tasks;


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
            Status = ProjectTaskStatus.ToDo,
            Priority = request.Priority,
        };
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return task.Id;
    }
}