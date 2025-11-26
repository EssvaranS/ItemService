using ItemService.Infrastructure.IRepositories;

namespace ItemService.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IItemRepository _itemRepository;

        public UnitOfWork(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public IItemRepository Items => _itemRepository;

        // For MongoDB there's typically nothing to commit in a SQL-transaction sense.
        // But if you use transactions (replica set), you can implement them here.
        public Task CommitAsync() => Task.CompletedTask;
    }
}
