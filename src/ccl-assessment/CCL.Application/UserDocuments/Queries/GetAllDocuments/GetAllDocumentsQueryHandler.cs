using CCL.Domain.Common;
using CCL.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CCL.Application.UserDocuments.Queries.GetAllDocuments;

public class GetAllDocumentsQueryHandler(IUserDocumentRepository repository, ILogger<GetAllDocumentsQueryHandler> logger) : IRequestHandler<GetAllDocumentsQuery, PagedResult<UserDocument>>
{
    public async Task<PagedResult<UserDocument>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        var searchContext = request.userDocumentSearch ?? new UserDocumentSearch();
        logger.LogInformation($"Getting all documents based on filter : {JsonSerializer.Serialize(searchContext)}");

        var (userDocs, totalCount) = await repository.GetAllAsync(searchContext);
        return new PagedResult<UserDocument>(userDocs, totalCount, searchContext.PageSize, searchContext.PageNumber);
    }
}
