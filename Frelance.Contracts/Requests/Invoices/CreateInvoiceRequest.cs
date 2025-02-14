using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.Invoices;

public record CreateInvoiceRequest(string ProjectName, string ClientName , decimal Amount, IFormFile InvoiceFile);