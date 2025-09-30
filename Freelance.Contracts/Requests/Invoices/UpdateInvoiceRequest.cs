using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.Invoices;

[UsedImplicitly]
public class UpdateInvoiceRequest(decimal amount, string status, string invoiceFile)
{
    public decimal? Amount { get; } = amount;
    public string? Status { get; } = status;
    public string? InvoiceFile { get; } = invoiceFile;
}