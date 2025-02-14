using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Proposals;
using Frelance.Application.Mediatr.Queries.Proposals;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class ProposalRepository : IProposalRepository
{
    private readonly FrelanceDbContext _dbContext;
    private readonly IUserAccessor _userAccessor;

    public ProposalRepository(FrelanceDbContext dbContext, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _dbContext = dbContext;
        _userAccessor = userAccessor;
    }

    public async Task AddProposalAsync(CreateProposalCommand createProposalCommand, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.
            AsNoTracking().
            FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(),
            cancellationToken);
        var project = await _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == createProposalCommand.CreateProposalRequest.ProjectName,
                cancellationToken);
        if (project == null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Title)}:{createProposalCommand.CreateProposalRequest.ProjectName} not found");
        }

        var proposal = new Proposals
        {
            ProjectId = project.Id,
            ProposerId = user.Id,
            ProposedBudget = createProposalCommand.CreateProposalRequest.ProposedBudget,
            Status = "Created",
            CreatedAt = DateTime.UtcNow,
        };
        await _dbContext.Proposals.AddAsync(proposal, cancellationToken);
    }

    public async Task<ProposalsDto> GetProposalByIdAsync(GetProposalByIdQuery getProposalByIdQuery, CancellationToken cancellationToken)
    {
        var proposal = await _dbContext.Proposals
            .AsNoTracking()
            .Include(x => x.Project)
            .Include(x => x.Proposer)
            .FirstOrDefaultAsync(x => x.Id == getProposalByIdQuery.Id, cancellationToken);

        if (proposal is null)
        {
            throw new NotFoundException($"{nameof(Proposals)}  with {nameof(Proposals.Id)}:{getProposalByIdQuery.Id} not found");
        }

        var proposalDto = proposal.Adapt<ProposalsDto>();
        return proposalDto;
    }

    public async Task<PaginatedList<ProposalsDto>> GetProposalsAsync(GetProposalsQuery getProposalsQuery, CancellationToken cancellationToken)
    {
        var proposalsQuery = _dbContext.Proposals
            .AsNoTracking()
            .Include(x => x.Proposer)
            .Include(x => x.Project)
            .ProjectToType<ProposalsDto>();

        var count = await proposalsQuery.CountAsync(cancellationToken);
        var items = await proposalsQuery
            .Skip((getProposalsQuery.PaginationParams.PageNumber - 1) * getProposalsQuery.PaginationParams.PageSize)
            .Take(getProposalsQuery.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ProposalsDto>(items, count, getProposalsQuery.PaginationParams.PageNumber, getProposalsQuery.PaginationParams.PageSize);
    }

    public async Task UpdateProposalAsync(UpdateProposalCommand updateProposalCommand, CancellationToken cancellationToken)
    {
        var proposalToUpdate = await _dbContext.Proposals
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == updateProposalCommand.Id, cancellationToken);

        if (proposalToUpdate is null)
        {
            throw new NotFoundException($"{nameof(Proposals)}  with {nameof(Proposals.Id)}:{updateProposalCommand.Id} not found");
        }
        proposalToUpdate.ProposedBudget = updateProposalCommand.UpdateProposalRequest.ProposedBudget;
        proposalToUpdate.Status = updateProposalCommand.UpdateProposalRequest.Status;
        proposalToUpdate.UpdatedAt = DateTime.UtcNow;
        _dbContext.Proposals.Update(proposalToUpdate);
    }

    public async Task DeleteProposalAsync(DeleteProposalCommand deleteProposalCommand, CancellationToken cancellationToken)
    {
        var proposalToDelete = await _dbContext.Proposals
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == deleteProposalCommand.Id, cancellationToken);

        if (proposalToDelete is null)
        {
            throw new NotFoundException($"{nameof(Proposals)}  with {nameof(Proposals.Id)}:{deleteProposalCommand.Id} not found");
        }
        _dbContext.Proposals.Remove(proposalToDelete);
    }
}