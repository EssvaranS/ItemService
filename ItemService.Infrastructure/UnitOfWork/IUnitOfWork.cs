using ItemService.Infrastructure.IRepositories;
namespace ItemService.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Unit of work interface for MongoDB transaction management and repository access.
    /// </summary>
    public interface IUnitOfWork
    {
        IItemRepository Items { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync();
    }
}
