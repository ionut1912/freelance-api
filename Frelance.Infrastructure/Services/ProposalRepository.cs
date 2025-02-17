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

    private readonly IUserAccessor _userAccessor;
    private readonly IGenericRepository<Proposals> _proposalRepository;
    private readonly IGenericRepository<Users> _userRepository;
    private readonly IGenericRepository<Projects> _projectRepository;

    public ProposalRepository(IUserAccessor userAccessor,
        IGenericRepository<Proposals> proposalRepository,
        IGenericRepository<Users> userRepository,
        IGenericRepository<Projects> projectRepository)
    {
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        ArgumentNullException.ThrowIfNull(proposalRepository, nameof(proposalRepository));
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        _userAccessor = userAccessor;
        _proposalRepository = proposalRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
    }

    public async Task AddProposalAsync(CreateProposalCommand createProposalCommand, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Query()
            .Where(x => x.UserName == _userAccessor.GetUsername())
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            throw new NotFoundException($"{nameof(Users)} with {nameof(Users.UserName)} {_userAccessor.GetUsername()} not found");
        }
        var project = await _projectRepository.Query()
            .Where(x => x.Title == createProposalCommand.CreateProposalRequest.ProjectName)
            .FirstOrDefaultAsync(cancellationToken);

        if (project == null)
        {
            throw new NotFoundException($"{nameof(Projects)} with {nameof(Projects.Title)}:{createProposalCommand.CreateProposalRequest.ProjectName} not found");
        }

        var proposal = createProposalCommand.CreateProposalRequest.Adapt<Proposals>();
        proposal.ProjectId = project.Id;
        proposal.ProposerId = user.Id;
        proposal.Status = "Created";
        await _proposalRepository.AddAsync(proposal, cancellationToken);
    }

    public async Task<ProposalsDto> GetProposalByIdAsync(GetProposalByIdQuery getProposalByIdQuery, CancellationToken cancellationToken)
    {
        var proposal = await _proposalRepository.Query()
            .Where(x => x.Id == getProposalByIdQuery.Id)
            .Include(x => x.Project)
            .Include(x => x.Proposer)
            .FirstOrDefaultAsync(cancellationToken);

        if (proposal is null)
        {
            throw new NotFoundException($"{nameof(Proposals)}  with {nameof(Proposals.Id)}:{getProposalByIdQuery.Id} not found");
        }

        var proposalDto = proposal.Adapt<ProposalsDto>();
        return proposalDto;
    }

    public async Task<PaginatedList<ProposalsDto>> GetProposalsAsync(GetProposalsQuery getProposalsQuery, CancellationToken cancellationToken)
    {
        var proposalsQuery = _proposalRepository.Query()
            .Include(x => x.Project)
            .Include(x => x.Proposer)
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
        var proposalToUpdate = await _proposalRepository.Query()
            .Where(x => x.Id == updateProposalCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (proposalToUpdate is null)
        {
            throw new NotFoundException($"{nameof(Proposals)}  with {nameof(Proposals.Id)}:{updateProposalCommand.Id} not found");
        }
        proposalToUpdate = updateProposalCommand.UpdateProposalRequest.Adapt<Proposals>();
        _proposalRepository.Update(proposalToUpdate);
    }

    public async Task DeleteProposalAsync(DeleteProposalCommand deleteProposalCommand, CancellationToken cancellationToken)
    {
        var proposalToDelete = await _proposalRepository.Query()
            .Where(x => x.Id == deleteProposalCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (proposalToDelete is null)
        {
            throw new NotFoundException($"{nameof(Proposals)}  with {nameof(Proposals.Id)}:{deleteProposalCommand.Id} not found");
        }
        _proposalRepository.Delete(proposalToDelete);
    }
}