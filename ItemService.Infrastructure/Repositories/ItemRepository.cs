using ItemService.Domain.Entities;
using ItemService.Infrastructure.IRepositories;
using MongoDB.Driver;

namespace ItemService.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for item data access using MongoDB.
    /// </summary>
    public class ItemRepository : IItemRepository
    {
        private readonly IMongoCollection<Item> _col;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRepository"/> class.
        /// </summary>
        /// <param name="context">MongoDB context.</param>
        public ItemRepository(IMongoDbContext context)
        {
            _col = context.Database.GetCollection<Item>("items");
        }

        /// <summary>
        /// Adds a new item entity to the collection.
        /// </summary>
        public async Task AddAsync(Item entity) => await _col.InsertOneAsync(entity);

        /// <summary>
        /// Deletes an item by its ID.
        /// </summary>
        public async Task<bool> DeleteAsync(string id)
        {
            var res = await _col.DeleteOneAsync(i => i.Id == id);
            return res.DeletedCount > 0;
        }

        /// <summary>
        /// Gets all items in the collection.
        /// </summary>
        public async Task<IEnumerable<Item>> GetAllAsync() => await _col.Find(_ => true).ToListAsync();

        /// <summary>
        /// Gets an item by its ID.
        /// </summary>
        public async Task<Item?> GetByIdAsync(string id) => await _col.Find(i => i.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Updates an item by its ID using a patch update definition.
        /// </summary>
        public async Task<bool> PatchAsync(string id, UpdateDefinition<Item> update)
        {
            var result = await _col.UpdateOneAsync(
                x => x.Id == id, update
            );

            return result.ModifiedCount > 0;
        }

    }
}
