using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelance.Domain.Entities;
using Frelance.API.Frelance.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.API.Frelance.Application.Commands.Tasks.UpdateTask;

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

        if (request.Title != null)
        {
            projectTaskToUpdate.Title = request.Title;
        }

        if (request.Description != null)
        {
            projectTaskToUpdate.Description = request.Description;
        }

        if (request.Status != null)
        {
            projectTaskToUpdate.Status = request.Status.Value;
        }
        if (request.Priority != null)
        {
            projectTaskToUpdate.Priority = request.Priority.Value;
        }

        _context.Tasks.Update(projectTaskToUpdate);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}