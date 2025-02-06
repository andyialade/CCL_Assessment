using CCL.UI.Models;

namespace CCL.UI.Services;

public class UserDocumentService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserDocumentService> _logger;

    public UserDocumentService(HttpClient httpClient, ILogger<UserDocumentService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PagedResult<UserDocument>> GetAllUserDocumentsAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Requesting all documents details from API");
        var response = await _httpClient.GetFromJsonAsync<PagedResult<UserDocument>>($"api/v1/UserDocument/all?userDocumentSearch.PageNumber={pageNumber}&userDocumentSearch.PageSize={pageSize}");
        return response != null ? response : new PagedResult<UserDocument>();
    }
}
