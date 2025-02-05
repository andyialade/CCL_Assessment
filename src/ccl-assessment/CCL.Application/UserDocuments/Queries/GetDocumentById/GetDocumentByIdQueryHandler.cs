namespace CCL.Application.UserDocuments.Queries.GetDocumentById;

public class GetDocumentByIdQueryHandler(IUserDocumentRepository repository) : IRequestHandler<GetDocumentByIdQuery, UserDocument>
{
    public async Task<UserDocument> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var userDocument = await repository.GetByIdAsync(request.id)
        ?? throw new NotFoundException(nameof(UserDocument), request.id.ToString());

        return userDocument;
    }
}
