using Frelance.API.Frelamce.Contracts;
using Frelance.API.Frelance.Domain.Entities;
using Frelance.API.Frelance.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Commands.Projects.UpdateProject;

public class UpdateProjectCommandHandler:IRequestHandler<UpdateProjectCommand,Unit>
{
    private readonly FrelanceDbContext _context;

    public UpdateProjectCommandHandler(FrelanceDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectToUpdate = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (projectToUpdate is null)
        {
            throw new NotFoundException($"{nameof(Project)} with {nameof(Project.Id)} : '{request.Id}' does not exist");
        }

        if (request.Description != null)
        {
            projectToUpdate.Description = request.Description;
        }

        if (request.Title != null)
        {
            projectToUpdate.Title = request.Title;
        }

        if (request.Deadline != null)
        {
            projectToUpdate.Deadline = request.Deadline.Value;
        }

        if (request.Technologies != null)
        {
            projectToUpdate.Technologies=request.Technologies;
        }

        if (request.Budget != null)
        {
            projectToUpdate.Budget = request.Budget.Value;
        }
        _context.Projects.Update(projectToUpdate);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}