using CCL.UI.Models;

namespace CCL.UI.Services;

public class UserDocumentService
{
    private readonly HttpClient _httpClient;

    public UserDocumentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDocument>> GetAllUserDocumentsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<PagedResult<UserDocument>>("https://localhost:7051/api/v1/UserDocument/all");

        return response.Items.Any() ? response.Items.ToList() : new List<UserDocument>();
    }
}
