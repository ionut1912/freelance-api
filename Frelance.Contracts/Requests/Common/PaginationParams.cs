namespace Frelance.Contracts.Requests.Common;

public class PaginationParams
{
    public int PageNumber { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaximumPageSize ? MaximumPageSize : value;
    }

    private readonly int _pageSize = 10;
    private const int MaximumPageSize = 100;
}