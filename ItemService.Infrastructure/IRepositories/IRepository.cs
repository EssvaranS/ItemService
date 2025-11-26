using ItemService.Domain.Entities;
using MongoDB.Driver;

namespace ItemService.Infrastructure.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task<bool> PatchAsync(string id, UpdateDefinition<Item> update);
        Task<bool> DeleteAsync(string id);
    }
}
