using CCL.Domain.Common;
using CCL.Domain.Models;

namespace CCL.Application.UserDocuments.Queries.GetAllDocuments;

public record GetAllDocumentsQuery(UserDocumentSearch? userDocumentSearch) : IRequest<PagedResult<UserDocument>>
{
}
