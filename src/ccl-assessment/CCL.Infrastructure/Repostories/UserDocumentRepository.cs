using CCL.Domain.Constants;
using CCL.Domain.Entities;
using CCL.Domain.Exceptions;
using CCL.Domain.Models;
using CCL.Domain.Repositories;
using CCL.Infrastructure.Converters;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Text.Json;

namespace CCL.Infrastructure.Repostories;

public class UserDocumentRepository(IConfiguration configuration) : IUserDocumentRepository
{
    public async Task<(IEnumerable<UserDocument>, int)> GetAllAsync(UserDocumentSearch userDocumentSearch)
    {
        string dbFilePath = configuration.GetSection("Database").GetSection("JsonData").Value ?? string.Empty;
        if (string.IsNullOrEmpty(dbFilePath)) throw new InternalServerErrorException("Unable to locate database. Please contact administrator");

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new DateTimeConverter() }
        };


        using (var stream = new FileStream(dbFilePath, FileMode.Open, FileAccess.Read))
        {
            var jsonData = await JsonSerializer.DeserializeAsync<IEnumerable<UserDocument>>(stream, options);
            if (jsonData == null || !jsonData.Any())
                return (Enumerable.Empty<UserDocument>(), 0);


            var baseQuery = jsonData.AsQueryable();

            var totalCount = baseQuery.Count();

            if (!string.IsNullOrEmpty(userDocumentSearch.FileName))
            {
                baseQuery = baseQuery.Where(x => x.FileName.ToLower().Contains(userDocumentSearch.FileName.ToLower()));
            }

            if (userDocumentSearch.IsEncrypted.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.IsEncrypted == userDocumentSearch.IsEncrypted.Value);
            }

            if (!string.IsNullOrEmpty(userDocumentSearch.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<UserDocument, object>>>
                {
                    { nameof(UserDocument.FileName), r => r.FileName },
                    { nameof(UserDocument.IsEncrypted), r => r.IsEncrypted }
                };

                var selectedColumn = columnsSelector[userDocumentSearch.SortBy];

                baseQuery = userDocumentSearch.SortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }
            else
            {
                baseQuery = userDocumentSearch.SortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(x => x.FileName)
                    : baseQuery.OrderByDescending(x => x.FileName);
            }

            var userDocs = baseQuery
                .Skip(userDocumentSearch.PageSize * (userDocumentSearch.PageNumber - 1))
                .Take(userDocumentSearch.PageSize)
                .ToList();

            return (userDocs, totalCount);
        }

    }

    public async Task<UserDocument?> GetByIdAsync(int id)
    {
        string dbFilePath = configuration.GetSection("Database").GetSection("JsonData").Value ?? string.Empty;
        if (string.IsNullOrEmpty(dbFilePath)) throw new InternalServerErrorException("Unable to locate database. Please contact administrator");

        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        var filename = Path.Combine(basePath, dbFilePath);

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new DateTimeConverter() }
        };

        using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            var jsonData = await JsonSerializer.DeserializeAsync<IEnumerable<UserDocument>>(stream, options);
            if (jsonData == null || !jsonData.Any())
                return null;

            return jsonData.FirstOrDefault(x => x.Index == id);
        }
    }
}
