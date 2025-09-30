namespace Freelance.Web.Extensions;

public static class ResultsExtensions
{
    public static IResult OkPaginationResult(this IResultExtensions resultExtensions,
        int pageSize, int pageNumber, int totalItems,
        int totalPages, IEnumerable<object> items)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions, nameof(resultExtensions));
        return new PaginationResult(pageSize, pageNumber, totalItems, totalPages, items);
    }
}