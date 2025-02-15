using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Contracts;
using Frelance.Application.Mediatr.Queries.Contracts;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.External;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class ContractRepository : IContractRepository
{
    private readonly IBlobService _blobService;
    private readonly FrelanceDbContext _dbContext;
    private readonly IUserAccessor _userAccessor;

    public ContractRepository(IBlobService blobService, FrelanceDbContext dbContext, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(blobService, nameof(blobService));
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _blobService = blobService;
        _dbContext = dbContext;
        _userAccessor = userAccessor;
    }

    public async Task AddContractAsync(CreateContractCommand createContractCommand, CancellationToken cancellationToken)
    {
        var freelancer = await _dbContext.FreelancerProfiles
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Users.UserName == createContractCommand.CreateContractRequest.FreelancerName, cancellationToken);
        if (freelancer is null)
        {
            throw new NotFoundException($"{nameof(FreelancerProfiles)} with {nameof(FreelancerProfiles.Users.UserName)}: {createContractCommand.CreateContractRequest.FreelancerName} doe not exist.");
        }
        var client = await _dbContext.ClientProfiles
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Users.UserName == _userAccessor.GetUsername(), cancellationToken);
        if (client is null)
        {
            throw new NotFoundException($"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Users.UserName)}: {_userAccessor.GetUsername()} doe not exist.");
        }
        var project = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Title == createContractCommand.CreateContractRequest.ProjectName, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Title)}: {createContractCommand.CreateContractRequest.ProjectName} doe not exist.");
        }

        var contract = createContractCommand.CreateContractRequest.Adapt<Entities.Contracts>();
        contract.ProjectId = project.Id;
        contract.ClientId = client.Id;
        contract.FreelancerId = freelancer.Id;
        contract.Status = "Signed";
        contract.ContractFileUrl = await _blobService.UploadBlobAsync(
            StorageContainers.CONTRACTSCONTAINER.ToString().ToLower(),
            $"{project.Id}/{createContractCommand.CreateContractRequest.ContractFile.FileName}",
            createContractCommand.CreateContractRequest.ContractFile);
        await _dbContext.Contracts.AddAsync(contract, cancellationToken);
    }

    public async Task<ContractsDto> GetContractByIdAsync(GetContractByIdQuery query, CancellationToken cancellationToken)
    {
        var contract = await _dbContext.Contracts
            .AsNoTracking()
            .Include(x => x.Project)
            .Include(x => x.Client)
            .ThenInclude(c => c.Users)
            .Include(x => x.Freelancer)
            .ThenInclude(f => f.Users)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);


        if (contract is null)
        {
            throw new NotFoundException($"{nameof(Entities.Contracts)} with {nameof(Entities.Contracts.Id)}: {query.Id} doe not exist.");
        }

        return contract.Adapt<ContractsDto>();
    }

    public async Task<PaginatedList<ContractsDto>> GetContractsAsync(GetContractsQuery query, CancellationToken cancellationToken)
    {
        var contractsQuery = _dbContext.Contracts
            .AsNoTracking()
            .Include(x => x.Client)
            .ThenInclude(x => x.Users)
            .Include(x => x.Freelancer)
            .ThenInclude(f => f.Users)
            .Include(x => x.Project)
            .ProjectToType<ContractsDto>();

        var count = await contractsQuery.CountAsync(cancellationToken);
        var items = await contractsQuery
            .Skip((query.PaginationParams.PageNumber - 1) * query.PaginationParams.PageSize)
            .Take(query.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ContractsDto>(items, count, query.PaginationParams.PageNumber, query.PaginationParams.PageSize);
    }

    public async Task UpdateContractAsync(UpdateContractCommand updateContractCommand, CancellationToken cancellationToken)
    {
        var contract = await _dbContext.Contracts
            .AsNoTracking()
            .Include(x => x.Project)
            .Include(x => x.Client)
            .ThenInclude(x => x.Projects)
            .Include(x => x.Freelancer)
            .ThenInclude(x => x.Projects)
            .FirstOrDefaultAsync(x => x.Id == updateContractCommand.Id, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException($"nameof(Entities.Contracts) with {nameof(Entities.Contracts.Id)}: {updateContractCommand.Id} doe not exist.");
        }

        updateContractCommand.UpdateContractRequest.Adapt(contract);

        if (updateContractCommand.UpdateContractRequest.ContractFile is not null)
        {
            await _blobService.DeleteBlobAsync(StorageContainers.CONTRACTSCONTAINER.ToString().ToLower(),
                contract.Project.Id.ToString());
            contract.ContractFileUrl =
                await _blobService.UploadBlobAsync(StorageContainers.CONTRACTSCONTAINER.ToString().ToLower(),
                    $"{contract.Project.Id}/{updateContractCommand.UpdateContractRequest.ContractFile.FileName}",
                    updateContractCommand.UpdateContractRequest.ContractFile);
        }

        if (updateContractCommand.UpdateContractRequest.Status == "Signed"
            && contract.Freelancer.Projects is not null
            && contract.Client.Projects is not null)
        {
            contract.Freelancer.Projects.Add(contract.Project);
            contract.Client.Projects.Add(contract.Project);
        }

        _dbContext.Contracts.Update(contract);
        _dbContext.ClientProfiles.Update(contract.Client);
        _dbContext.FreelancerProfiles.Update(contract.Freelancer);
    }



    public async Task DeleteContractAsync(DeleteContractCommand deleteContractCommand, CancellationToken cancellationToken)
    {
        var contractToDelete = await _dbContext.Contracts
            .AsNoTracking()
            .Include(x => x.Project)
            .FirstOrDefaultAsync(x => x.Id == deleteContractCommand.Id, cancellationToken);
        if (contractToDelete is null)
        {
            throw new NotFoundException($"{nameof(Entities.Contracts)} with {nameof(Entities.Contracts.Id)}: {deleteContractCommand.Id} doe not exist.");
        }

        await _blobService.DeleteBlobAsync(StorageContainers.CONTRACTSCONTAINER.ToString().ToLower(),
            contractToDelete.Project.Id.ToString());
        _dbContext.Contracts.Remove(contractToDelete);
    }
}