namespace Frelance.Contracts.Responses.Common;

public class PaginatedList<T>(IEnumerable<T> items, int count, int pageNumber, int pageSize)
{
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = count;
    public int CurrentPage { get; set; } = pageNumber;
    public IEnumerable<T> Items { get; set; } = items;
}