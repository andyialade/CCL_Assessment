namespace CCL.Domain.Models;

public class UserDocumentSearch : PaginationProps
{
    public string? FileName { get; set; }

    public bool? IsEncrypted { get; set; }

    public UserDocumentSearch()
    {
        FileName = null;
        IsEncrypted = null;
        PageNumber = 1;
        PageSize = 10;
        SortBy = string.Empty;
        SortDirection = Constants.SortDirection.Ascending;
    }
}
