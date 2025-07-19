using Frelance.Application.Mediatr.Commands.Invoices;
using Frelance.Application.Mediatr.Queries.Invoices;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses.Common;
using Frelance.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Frelance.Infrastructure.Services;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly IGenericRepository<ClientProfiles> _clientProfileRepository;
    private readonly IGenericRepository<FreelancerProfiles> _freelancerProfileRepository;
    private readonly IGenericRepository<Invoices> _invoiceRepository;
    private readonly IGenericRepository<Projects> _projectRepository;
    private readonly IUserAccessor _userAccessor;

    public InvoiceRepository(
        IUserAccessor userAccessor,
        IGenericRepository<Invoices> invoiceRepository,
        IGenericRepository<ClientProfiles> clientProfileRepository,
        IGenericRepository<FreelancerProfiles> freelancerProfileRepository,
        IGenericRepository<Projects> projectRepository)
    {
        ArgumentNullException.ThrowIfNull(userAccessor, nameof(userAccessor));
        ArgumentNullException.ThrowIfNull(invoiceRepository, nameof(invoiceRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(projectRepository, nameof(projectRepository));
        _userAccessor = userAccessor;
        _invoiceRepository = invoiceRepository;
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
        _projectRepository = projectRepository;
    }

    public async Task CreateInvoiceAsync(CreateInvoiceCommand createInvoiceCommand, CancellationToken cancellationToken)
    {
        var client = await _clientProfileRepository.Query()
            .Where(x => x.Users!.UserName == createInvoiceCommand.CreateInvoiceRequest.ClientName)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(cancellationToken);

        if (client is null)
            throw new NotFoundException(
                $"{nameof(ClientProfiles)} with {nameof(ClientProfiles.Users.UserName)}: {createInvoiceCommand.CreateInvoiceRequest.ClientName} not foun");


        var freelancer = await _freelancerProfileRepository.Query()
            .Where(x => x.Users!.UserName == _userAccessor.GetUsername())
            .Include(x => x.Users)
            .FirstOrDefaultAsync(cancellationToken);

        if (freelancer is null)
            throw new NotFoundException(
                $"{nameof(FreelancerProfiles)} with {nameof(ClientProfiles.Users.UserName)}: {_userAccessor.GetUsername()} not found");

        var project = await _projectRepository.Query()
            .Where(x => x.Title == createInvoiceCommand.CreateInvoiceRequest.ProjectName)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(
                $"{nameof(Projects)} with {nameof(Projects.Title)}: {createInvoiceCommand.CreateInvoiceRequest.ProjectName} not found");
        var invoice = createInvoiceCommand.CreateInvoiceRequest.Adapt<Invoices>();
        invoice.ProjectId = project.Id;
        invoice.ClientId = client.Id;
        invoice.FreelancerId = freelancer.Id;
        invoice.Status = "Submitted";
        await _invoiceRepository.CreateAsync(invoice, cancellationToken);
    }

    public async Task<InvoicesDto> GetInvoiceByIdAsync(GetInvoiceByIdQuery query, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.Query()
            .Where(x => x.Id == query.Id)
            .Include(x => x.Project)
            .Include(x => x.Client)
            .ThenInclude(x => x!.Users)
            .Include(x => x.Freelancer)
            .ThenInclude(x => x!.Users)
            .FirstOrDefaultAsync(cancellationToken);

        return invoice is null
            ? throw new NotFoundException($"{nameof(Invoices)} with {nameof(Invoices.Id)}: {query.Id} not found")
            : invoice.Adapt<InvoicesDto>();
    }

    public async Task<PaginatedList<InvoicesDto>> GetInvoicesAsync(GetInvoicesQuery query,
        CancellationToken cancellationToken)
    {
        var invoicesQuery = _invoiceRepository.Query()
            .Include(x => x.Client)
            .ThenInclude(x => x!.Users)
            .Include(x => x.Freelancer)
            .ThenInclude(x => x!.Users)
            .Include(x => x.Project)
            .ProjectToType<InvoicesDto>();

        var count = await invoicesQuery.CountAsync(cancellationToken);
        var items = await invoicesQuery
            .Skip((query.PaginationParams.PageNumber - 1) * query.PaginationParams.PageSize)
            .Take(query.PaginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<InvoicesDto>(items, count, query.PaginationParams.PageNumber,
            query.PaginationParams.PageSize);
    }

    public async Task UpdateInvoiceAsync(UpdateInvoiceCommand updateInvoiceCommand, CancellationToken cancellationToken)
    {
        var invoiceToUpdate = await _invoiceRepository.Query()
            .Where(x => x.Id == updateInvoiceCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (invoiceToUpdate is null)
            throw new NotFoundException(
                $"{nameof(Invoices)} with {nameof(Invoices.Id)}: {updateInvoiceCommand.Id} not found");
        updateInvoiceCommand.UpdateInvoiceRequest.Adapt(invoiceToUpdate);
        _invoiceRepository.Update(invoiceToUpdate);
    }

    public async Task DeleteInvoiceAsync(DeleteInvoiceCommand deleteInvoiceCommand, CancellationToken cancellationToken)
    {
        var invoiceToDelete = await _invoiceRepository.Query()
            .Where(x => x.Id == deleteInvoiceCommand.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (invoiceToDelete is null)
            throw new NotFoundException(
                $"{nameof(Invoices)} with {nameof(Invoices.Id)}: {deleteInvoiceCommand.Id} not found");
        _invoiceRepository.Delete(invoiceToDelete);
    }
}