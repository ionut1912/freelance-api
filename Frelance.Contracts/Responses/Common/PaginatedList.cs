namespace Frelance.Contracts.Responses.Common;

public class PaginatedList<T>(IEnumerable<T> items, int count, int pageNumber, int pageSize)
{
    public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = count;
    public int CurrentPage { get; } = pageNumber;
    public IEnumerable<T> Items { get; } = items;
}