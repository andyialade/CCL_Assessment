using CCL.Domain.Common;
using CCL.Domain.Models;

namespace CCL.Application.UserDocuments.Queries.GetAllDocuments;

public class GetAllDocumentsQueryHandler(IUserDocumentRepository repository) : IRequestHandler<GetAllDocumentsQuery, PagedResult<UserDocument>>
{
    public async Task<PagedResult<UserDocument>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        var searchContext = request.userDocumentSearch ?? new UserDocumentSearch();
        var (userDocs, totalCount) = await repository.GetAllAsync(searchContext);
        return new PagedResult<UserDocument>(userDocs, totalCount, searchContext.PageSize, searchContext.PageNumber);
    }
}
