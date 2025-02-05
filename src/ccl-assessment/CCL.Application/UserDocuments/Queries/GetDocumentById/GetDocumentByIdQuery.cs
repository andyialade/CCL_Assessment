namespace CCL.Application.UserDocuments.Queries.GetDocumentById;

public record GetDocumentByIdQuery(int id) : IRequest<UserDocument>
{
}
