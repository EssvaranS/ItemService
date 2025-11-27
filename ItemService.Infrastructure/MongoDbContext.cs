using ItemService.Infrastructure.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemService.Infrastructure
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
        Task<bool> PingAsync();
    }

    public class MongoDbContext : IMongoDbContext
    {   
        private readonly IMongoClient _client;
        public IMongoDatabase Database { get; }

        public MongoDbContext(MongoOptions settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            Database = _client.GetDatabase(settings.DatabaseName);
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                var command = new BsonDocument("ping", 1);
                await Database.RunCommandAsync<BsonDocument>(command);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
