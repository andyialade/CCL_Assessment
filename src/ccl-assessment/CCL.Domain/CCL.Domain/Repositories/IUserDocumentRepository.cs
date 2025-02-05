using CCL.Domain.Entities;
using CCL.Domain.Models;

namespace CCL.Domain.Repositories;

public interface IUserDocumentRepository
{
    Task<UserDocument?> GetByIdAsync(int id);

    Task<(IEnumerable<UserDocument>, int)> GetAllAsync(UserDocumentSearch userDocumentSearch);
}
