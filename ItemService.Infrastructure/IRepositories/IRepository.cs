using ItemService.Domain.Entities;
using MongoDB.Driver;

namespace ItemService.Infrastructure.IRepositories
{
    /// <summary>
    /// Generic repository interface for MongoDB CRUD operations.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        Task<T?> GetByIdAsync(string id);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an entity by its ID using a patch update definition.
        /// </summary>
        Task<bool> PatchAsync(string id, UpdateDefinition<Item> update);

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        Task<bool> DeleteAsync(string id);
    }
}
