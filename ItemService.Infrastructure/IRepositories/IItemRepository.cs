using ItemService.Domain.Entities;

namespace ItemService.Infrastructure.IRepositories
{
    public interface IItemRepository : IRepository<Item>
    {
        // Put item-specific queries here
    }
}
