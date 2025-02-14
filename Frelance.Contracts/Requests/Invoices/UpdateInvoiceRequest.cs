using Microsoft.AspNetCore.Http;

namespace Frelance.Contracts.Requests.Invoices;

public class UpdateInvoiceRequest
{
    public decimal Amount { get; set; }
    public IFormFile? InvoiceFile { get; set; }
    public string Status { get; set; }

};