using Frelance.Application.Helpers;
using Frelance.Application.Mediatr.Commands.Invoices;
using Frelance.Application.Mediatr.Queries.Invoices;
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

public class InvoiceRepository : IInvoiceRepository
{
    private readonly FrelanceDbContext _frelanceDbContext;
    private readonly IBlobService _blobService;
    private readonly IUserAccessor _userAccessor;

    public InvoiceRepository(FrelanceDbContext frelanceDbContext, IBlobService blobService, IUserAccessor userAccessor)
    {
        ArgumentNullException.ThrowIfNull(frelanceDbContext, nameof(frelanceDbContext));
        ArgumentNullException.ThrowIfNull(blobService, nameof(blobService));
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        _frelanceDbContext = frelanceDbContext;
        _blobService = blobService;
        _userAccessor = userAccessor;
    }

    public async Task AddInvoiceAsync(CreateInvoiceCommand createInvoiceCommand, CancellationToken cancellationToken)
    {
        var client = await _frelanceDbContext.ClientProfiles
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Users.UserName == createInvoiceCommand.CreateInvoiceRequest.ClientName, cancellationToken);
        if (client is null)
        {
            throw new NotFoundException($"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Users.UserName)}: {createInvoiceCommand.CreateInvoiceRequest.ClientName} not foun");
        }

        var freelancer = await _frelanceDbContext.FreelancerProfiles
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Users.UserName == _userAccessor.GetUsername(), cancellationToken);
        if (freelancer is null)
        {
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(ClientProfiles.Users.UserName)}: {_userAccessor.GetUsername()} not found");
        }
        var project = await _frelanceDbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == createInvoiceCommand.CreateInvoiceRequest.ProjectName, cancellationToken);

        if (project is null)
        {
            throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Title)}: {createInvoiceCommand.CreateInvoiceRequest.ProjectName} not found");
        }

        var invoice = createInvoiceCommand.Adapt<Invoices>();
        invoice.ProjectId = project.Id;
        invoice.ClientId = client.Id;
        invoice.FreelancerId = freelancer.Id;
        invoice.InvoiceFileUrl = await _blobService.UploadBlobAsync(
            StorageContainers.INVOICESCONTAINER.ToString().ToLower(),
            $"{project.Id}/{createInvoiceCommand.CreateInvoiceRequest.InvoiceFile.FileName}",
            createInvoiceCommand.CreateInvoiceRequest.InvoiceFile);
        invoice.Status = "Submitted";
        await _frelanceDbContext.Invoices.AddAsync(invoice, cancellationToken);


    }

    public async Task<InvoicesDto> GetInvoiceByIdAsync(GetInvoiceByIdQuery query, CancellationToken cancellationToken)
    {
        var invoice = await _frelanceDbContext.Invoices
            .AsNoTracking()
            .Include(x => x.Project)
            .Include(x => x.Client)
            .ThenInclude(x => x.Users)
            .Include(x => x.Freelancer)
            .ThenInclude(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
        if (invoice is null)
        {
            throw new NotFoundException($"{nameof(Invoices)} with {nameof(Invoices.Id)}: {query.Id} not found");
        }

        return invoice.Adapt<InvoicesDto>();
    }

    public async Task<PaginatedList<InvoicesDto>> GetInvoicesAsync(GetInvoicesQuery query, CancellationToken cancellationToken)
    {
        var invoicesQuery = _frelanceDbContext.Invoices
            .AsNoTracking()
            .Include(x => x.Client)
            .ThenInclude(x => x.Users)
            .Include(x => x.Freelancer)
            .ThenInclude(f => f.Users)
            .Include(x => x.Project)
            .ProjectToType<InvoicesDto>();

        var count = await invoicesQuery.CountAsync(cancellationToken);
        var items = await invoicesQuery
            .Skip((query.PaginationParams.PageNumber - 1) * query.PaginationParams.PageSize)
            .Take(query.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<InvoicesDto>(items, count, query.PaginationParams.PageNumber, query.PaginationParams.PageSize);
    }

    public async Task UpdateInvoiceAsync(UpdateInvoiceCommand updateInvoiceCommand, CancellationToken cancellationToken)
    {
        var invoiceToUpdate = await _frelanceDbContext.Invoices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == updateInvoiceCommand.Id, cancellationToken);
        if (invoiceToUpdate is null)
        {
            throw new NotFoundException($"{nameof(Invoices)} with {nameof(Invoices.Id)}: {updateInvoiceCommand.Id} not found");
        }
        invoiceToUpdate = updateInvoiceCommand.Adapt<Invoices>();
        if (updateInvoiceCommand.UpdateInvoiceRequest.InvoiceFile is not null)
        {
            await _blobService.DeleteBlobAsync(StorageContainers.INVOICESCONTAINER.ToString().ToLower(),
                invoiceToUpdate.ProjectId.ToString());
            invoiceToUpdate.InvoiceFileUrl = await _blobService.UploadBlobAsync(
                StorageContainers.INVOICESCONTAINER.ToString().ToLower(),
                $"{invoiceToUpdate.ProjectId}/{updateInvoiceCommand.UpdateInvoiceRequest.InvoiceFile}",
                updateInvoiceCommand.UpdateInvoiceRequest.InvoiceFile);
        }
        _frelanceDbContext.Invoices.Update(invoiceToUpdate);
    }

    public async Task DeleteInvoiceAsync(DeleteInvoiceCommand deleteInvoiceCommand, CancellationToken cancellationToken)
    {
        var invoiceToDelete = await _frelanceDbContext.Invoices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == deleteInvoiceCommand.Id, cancellationToken);
        if (invoiceToDelete is null)
        {
            throw new NotFoundException($"{nameof(Invoices)} with {nameof(Invoices.Id)}: {deleteInvoiceCommand.Id} not found");
        }
        await _blobService.DeleteBlobAsync(StorageContainers.INVOICESCONTAINER.ToString().ToLower(), invoiceToDelete.ProjectId.ToString());
        _frelanceDbContext.Invoices.Remove(invoiceToDelete);
    }
}