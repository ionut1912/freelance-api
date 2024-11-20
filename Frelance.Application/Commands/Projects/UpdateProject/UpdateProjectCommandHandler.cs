using MediatR;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Frelance.Contracts.Exceptions;
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
        projectToUpdate.Description = request.Description;
        projectToUpdate.Title = request.Title;
        projectToUpdate.Deadline = request.Deadline;
        projectToUpdate.Technologies=request.Technologies;
        projectToUpdate.Budget = request.Budget;
        _context.Projects.Update(projectToUpdate);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}