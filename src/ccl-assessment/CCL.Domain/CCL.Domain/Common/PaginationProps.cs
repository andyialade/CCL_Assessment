using CCL.Domain.Constants;

namespace CCL.Domain.Models;

public class PaginationProps
{
    public int PageSize { get; set; } = 10;

    public int PageNumber { get; set; } = 1;

    public string SortBy { get; set; } = default!;

    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}
