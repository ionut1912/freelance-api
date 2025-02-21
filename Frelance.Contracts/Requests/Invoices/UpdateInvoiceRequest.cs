namespace Frelance.Contracts.Requests.Invoices;

public class UpdateInvoiceRequest
{
    public decimal? Amount { get; set; }
    public string? Status { get; set; }
    public string? InvoiceFile { get; set; }
}