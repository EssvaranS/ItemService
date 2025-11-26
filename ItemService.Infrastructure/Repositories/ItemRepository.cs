using ItemService.Domain.Entities;
using ItemService.Infrastructure.IRepositories;
using MongoDB.Driver;

namespace ItemService.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMongoCollection<Item> _col;

        public ItemRepository(IMongoDbContext context)
        {
            _col = context.Database.GetCollection<Item>("items");
        }

        public async Task AddAsync(Item entity) => await _col.InsertOneAsync(entity);

        public async Task<bool> DeleteAsync(string id)
        {
            var res = await _col.DeleteOneAsync(i => i.Id == id);
            return res.DeletedCount > 0;
        }

        public async Task<IEnumerable<Item>> GetAllAsync() => await _col.Find(_ => true).ToListAsync();

        public async Task<Item?> GetByIdAsync(string id) => await _col.Find(i => i.Id == id).FirstOrDefaultAsync();

        public async Task<bool> PatchAsync(string id, UpdateDefinition<Item> update)
        {
            var result = await _col.UpdateOneAsync(
                x => x.Id == id, update
            );

            return result.ModifiedCount > 0;
        }

    }
}
