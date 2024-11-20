using MediatR;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Frelance.Contracts.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace Frelance.Application.Commands.Tasks.UpdateTask;

public class UpdateTaskCommandHandler:IRequestHandler<UpdateTaskCommand,Unit>
{
    private readonly FrelanceDbContext _context;

    public UpdateTaskCommandHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTaskToUpdate= await _context.Tasks.FirstOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
        if (projectTaskToUpdate is null)
        {
            throw new NotFoundException($"{nameof(ProjectTask)} with {nameof(ProjectTask.Id)} : '{request.Id}' does not exist");
        }
        
        projectTaskToUpdate.Title = request.Title;
        projectTaskToUpdate.Description = request.Description;
        projectTaskToUpdate.Status = request.Status;
        projectTaskToUpdate.Priority = request.Priority;
        
        _context.Tasks.Update(projectTaskToUpdate);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}