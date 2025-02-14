using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.Invoices;

public record CreateInvoiceRequest(string ProjectName, string ClientName, DateOnly Date, decimal Amount, IFormFile InvoiceFile);