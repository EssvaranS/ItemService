using ItemService.Infrastructure.IRepositories;
using MongoDB.Driver;

namespace ItemService.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implements the unit of work pattern for MongoDB repositories.
    /// </summary>
    /// <summary>
    /// Implements the unit of work pattern for MongoDB repositories with transaction support.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMongoDbContext _dbContext;
        private IClientSessionHandle? _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="itemRepository">Item repository instance.</param>
        /// <param name="dbContext">MongoDB context for transaction management.</param>
        public UnitOfWork(IItemRepository itemRepository, IMongoDbContext dbContext)
        {
            _itemRepository = itemRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the item repository.
        /// </summary>
        public IItemRepository Items => _itemRepository;

        /// <summary>
        /// Begins a MongoDB transaction session.
        /// </summary>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_session != null)
                throw new InvalidOperationException("Transaction has already been started.");
            var client = (_dbContext.Database.Client as MongoDB.Driver.MongoClient);
            if (client == null)
                throw new InvalidOperationException("MongoDB client not available.");
            _session = await client.StartSessionAsync(cancellationToken: cancellationToken);
            _session.StartTransaction();
        }

        /// <summary>
        /// Commits the MongoDB transaction.
        /// </summary>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_session == null)
                throw new InvalidOperationException("Transaction has not been started.");
            await _session.CommitTransactionAsync(cancellationToken);
            _session.Dispose();
            _session = null;
        }

        /// <summary>
        /// Rolls back the MongoDB transaction.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_session != null)
            {
                await _session.AbortTransactionAsync();
                _session.Dispose();
                _session = null;
            }
        }
    }
}
