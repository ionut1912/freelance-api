using Frelance.Domain.Entities;
using Frelance.Infrastructure;
using MediatR;

namespace Frelance.Application.Commands.Projects.CreateProject;

public class CreateProjectCommandHandler:IRequestHandler<CreateProjectCommand,int>
{
    public readonly FrelanceDbContext _frelanceDbContext;

    public CreateProjectCommandHandler(FrelanceDbContext frelanceDbContext)
    {
        _frelanceDbContext = frelanceDbContext;
    }

    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            CreatedAt = DateTime.Now.ToUniversalTime(),
            Title = request.Title,
            Description = request.Description,
            Deadline = request.Deadline,
            Technologies = request.Technologies
        };
        await _frelanceDbContext.Projects.AddAsync(project,cancellationToken);
        await _frelanceDbContext.SaveChangesAsync(cancellationToken);
        return project.Id;
    }
    
}