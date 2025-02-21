namespace Frelance.Contracts.Requests.Common;

public class PaginationParams
{
    private const int MaximumPageSize = 100;

    private readonly int _pageSize = 10;
    public int PageNumber { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaximumPageSize ? MaximumPageSize : value;
    }
}