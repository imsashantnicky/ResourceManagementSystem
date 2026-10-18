using ResourceManagementSystem.Domain.Entities;

namespace ResourceManagementSystem.Application.Repository;

public interface IRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task UpsertAsync(T item, CancellationToken ct = default);
}

