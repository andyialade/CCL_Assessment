using Microsoft.Extensions.Logging;

namespace CCL.Application.UserDocuments.Queries.GetDocumentById;

public class GetDocumentByIdQueryHandler(IUserDocumentRepository repository, ILogger<GetDocumentByIdQueryHandler> logger) : IRequestHandler<GetDocumentByIdQuery, UserDocument>
{
    public async Task<UserDocument> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting document where Id {request.id}");

        var userDocument = await repository.GetByIdAsync(request.id)
        ?? throw new NotFoundException(nameof(UserDocument), request.id.ToString());

        return userDocument;
    }
}
