using Bogus;
using CCL.Application.UserDocuments.Queries.GetAllDocuments;
using CCL.Domain.Entities;
using CCL.Domain.Models;
using CCL.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace CCL.ApplicationTests.UserDocuments.Queries.GetAllDocuments;

public class GetAllDocumentsQueryHandlerTests
{
    private Mock<IUserDocumentRepository> _mockUserDocumentRepository = new();
    private Mock<ILogger<GetAllDocumentsQueryHandler>> _mockLogger = new();
    private Faker _fakerObject = new Faker("en");


    [Fact]
    public async void Handle_GetAllDocumentsQueryHandler_DefaultFilters_ReturnsItemsList()
    {
        //arrange
        var query = new GetAllDocumentsQuery(null);
        var totalCount = _fakerObject.Random.Int(10, 20);
        var userDocuments = new List<UserDocument>()
        {
            new UserDocument()
            {
                Index = _fakerObject.Random.Int(1, 20),
                FileName = _fakerObject.System.FileName(),
                FullPath = _fakerObject.System.FilePath(),
                IsEncrypted = true,
                Created = _fakerObject.Date.Past(),
                Modified = _fakerObject.Date.Soon(),
            },
            new UserDocument()
            {
                Index = _fakerObject.Random.Int(1, 20),
                FileName = _fakerObject.System.FileName(),
                FullPath = _fakerObject.System.FilePath(),
                IsEncrypted = true,
                Created = _fakerObject.Date.Past(),
                Modified = _fakerObject.Date.Soon(),
            }
        };
        _mockUserDocumentRepository.Setup(m => m.GetAllAsync(It.IsAny<UserDocumentSearch>())).ReturnsAsync((userDocuments, totalCount));
        var _querytHandler = new GetAllDocumentsQueryHandler(_mockUserDocumentRepository.Object, _mockLogger.Object);

        //act
        var result = await _querytHandler.Handle(query, CancellationToken.None);

        //assert
        Assert.NotEmpty(result.Items);
    }


    [Theory()]
    [InlineData("document0", null)]
    [InlineData("", false)]
    [InlineData("document0", true)]
    [InlineData("document0", false)]
    public async void Handle_GetAllDocumentsQueryHandler_DifferentFilters_ReturnsItemsList(string fileName, bool? isEncrypted)
    {
        //arrange
        var query = new GetAllDocumentsQuery(new UserDocumentSearch() { FileName = fileName, IsEncrypted = isEncrypted });
        var totalCount = _fakerObject.Random.Int(10, 20);
        var userDocuments = new List<UserDocument>()
        {
            new UserDocument()
            {
                Index = _fakerObject.Random.Int(1, 20),
                FileName = !string.IsNullOrEmpty(fileName) ? fileName :  _fakerObject.System.FileName(),
                FullPath = _fakerObject.System.FilePath(),
                IsEncrypted = isEncrypted.HasValue ? isEncrypted.Value : false,
                Created = _fakerObject.Date.Past(),
                Modified = _fakerObject.Date.Soon(),
            },
            new UserDocument()
            {
                Index = _fakerObject.Random.Int(1, 20),
                FileName = _fakerObject.System.FileName(),
                FullPath = _fakerObject.System.FilePath(),
                IsEncrypted = false,
                Created = _fakerObject.Date.Past(),
                Modified = _fakerObject.Date.Soon(),
            }
        };
        _mockUserDocumentRepository.Setup(m => m.GetAllAsync(It.IsAny<UserDocumentSearch>())).ReturnsAsync((userDocuments, totalCount));
        var _querytHandler = new GetAllDocumentsQueryHandler(_mockUserDocumentRepository.Object, _mockLogger.Object);

        //act
        var result = await _querytHandler.Handle(query, CancellationToken.None);

        //assert
        Assert.NotEmpty(result.Items);
    }
}
