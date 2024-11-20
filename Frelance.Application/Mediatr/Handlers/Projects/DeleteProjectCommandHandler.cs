using Frelance.Application.Mediatr.Commands.Projects;
using Frelance.Contracts.Exceptions;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Application.Mediatr.Handlers.Projects;

public class DeleteProjectCommandHandler:IRequestHandler<DeleteProjectCommand,Unit>
{
    private readonly FrelanceDbContext _context;

    public DeleteProjectCommandHandler(FrelanceDbContext context)
    {
        _context = context;
        
    }
    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var projectToDelete = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (projectToDelete is null)
        {
            throw new NotFoundException($"{nameof(Project)} with {nameof(Project.Id)} : '{request.Id}' does not exist");
        }
        _context.Projects.Remove(projectToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        return  Unit.Value;
    }
}