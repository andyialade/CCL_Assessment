using Bogus;
using CCL.Domain.Common;
using CCL.Domain.Entities;
using CCL.Domain.Models;
using CCL.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace CCL.APITests
{
    public class UserDocumentControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly Mock<IUserDocumentRepository> _mockUserDocumentRepository = new();
        private readonly Faker _fakerObject;
        private readonly string _baseApiUrl;

        public UserDocumentControllerTests(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.Replace(ServiceDescriptor.Scoped(typeof(IUserDocumentRepository),
                                                _ => _mockUserDocumentRepository.Object));
                });
            });
            _fakerObject = new Faker("en");
            _baseApiUrl = "/api/v1/UserDocument";
        }

        [Fact]
        public async Task GetById_ForExistingId_ShouldReturn200Ok()
        {
            // arrange
            var id = _fakerObject.Random.Int(1, 20);
            var userDocument = new UserDocument()
            {
                Index = id,
                FileName = _fakerObject.System.FileName(),
                FullPath = _fakerObject.System.FilePath(),
                IsEncrypted = true,
                Created = _fakerObject.Date.Past(),
                Modified = _fakerObject.Date.Soon(),
            };

            var client = _applicationFactory.CreateClient();
            _mockUserDocumentRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(userDocument);

            // act
            var response = await client.GetAsync($"{_baseApiUrl}/{id}");
            var userDocumentObj = await response.Content.ReadFromJsonAsync<UserDocument>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(userDocumentObj);
        }

        [Fact]
        public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
        {
            // arrange
            var id = _fakerObject.Random.Int(1, 20);
            var client = _applicationFactory.CreateClient();
            _mockUserDocumentRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserDocument?)null);

            // act
            var response = await client.GetAsync($"{_baseApiUrl}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WithDefaultFilters_ShouldReturn200Ok()
        {
            // arrange
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

            var totalCount = 10;

            var client = _applicationFactory.CreateClient();
            _mockUserDocumentRepository.Setup(m => m.GetAllAsync(It.IsAny<UserDocumentSearch>())).ReturnsAsync((userDocuments, totalCount));

            // act
            var response = await client.GetAsync($"{_baseApiUrl}/all");
            var userDocumentObjs = await response.Content.ReadFromJsonAsync<PagedResult<UserDocument>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(userDocumentObjs.Items);
        }
    }
}