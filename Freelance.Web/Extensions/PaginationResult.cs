using System.Text.Json;

namespace Freelance.Web.Extensions;

public class PaginationResult(
    int pageSize,
    int pageNumber,
    int totalItems,
    int totalPages,
    IEnumerable<object> items)
    : IResult
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        var header = new
        {
            pageSize,
            pageNumber,
            totalItems,
            totalPages
        };

        httpContext.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(header));
        httpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        await httpContext.Response.WriteAsJsonAsync(items);
    }
}