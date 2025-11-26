using ItemService.Infrastructure.IRepositories;
namespace ItemService.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IItemRepository Items { get; }
        Task CommitAsync();
    }
}
