using ItemService.Application.DTOs;
using ItemService.Application.IServices;
using ItemService.Domain.Entities;
using ItemService.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ItemService.Application.Services
{
    /// <summary>
    /// Service for item business logic and data access.
    /// </summary>
    public class ItemServices : IItemService
    {
        private readonly IUnitOfWork _uow;
        private ILogger<ItemServices> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemServices"/> class.
        /// </summary>
        /// <param name="uow">Unit of work for repositories.</param>
        /// <param name="log">Logger instance.</param>
        public ItemServices(IUnitOfWork uow, ILogger<ItemServices> log)
        {
            _uow = uow;
            _log = log;
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="dto">Item creation data.</param>
        /// <returns>Created item DTO.</returns>
        public async Task<ItemDto> CreateAsync(CreateItemDto dto)
        {
            var now = DateTime.UtcNow;
            var entity = new Item
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CreatedAt = now,
                UpdatedAt = now
            };
            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.Items.AddAsync(entity); // Add item to repository
                await _uow.CommitAsync(); // Commit transaction
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
            return new ItemDto(entity.Id, entity.Name, entity.Description, entity.Price);
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>Enumerable of item DTOs.</returns>
        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _uow.Items.GetAllAsync(); // Get all items
            return items.Select(i => new ItemDto(i.Id, i.Name, i.Description, i.Price));
        }

        /// <summary>
        /// Gets an item by its ID.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <returns>Item DTO or null.</returns>
        public async Task<ItemDto?> GetByIdAsync(string id)
        {
            var item = await _uow.Items.GetByIdAsync(id); // Get item by ID
            if (item == null) return null;
            return new ItemDto(item.Id, item.Name, item.Description, item.Price);
        }

        /// <summary>
        /// Updates an item by its ID.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <param name="dto">Update data.</param>
        /// <returns>Updated item DTO or null.</returns>
        public async Task<ItemDto?> UpdateAsync(string id, UpdateItemDto dto)
        {
            var updates = new List<UpdateDefinition<Item>>();

            var properties = typeof(UpdateItemDto).GetProperties(); // Get properties to update

            foreach (var prop in properties)
            {
                var value = prop.GetValue(dto);
                if (value != null)
                {
                    updates.Add(
                        Builders<Item>.Update.Set(prop.Name, value)
                    );
                }
            }

            // Always update the UpdatedAt timestamp
            updates.Add(Builders<Item>.Update.Set("UpdatedAt", DateTime.UtcNow));

            if (!updates.Any())
                return null; // No values provided ⇒ Not updated

            var updateDefinition = Builders<Item>.Update.Combine(updates); // Combine updates

            await _uow.BeginTransactionAsync();
            try
            {
                var result = await _uow.Items.PatchAsync(id, updateDefinition); // Apply patch
                if (!result)
                {
                    await _uow.RollbackAsync();
                    return null; // Item not found case
                }
                await _uow.CommitAsync();
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }

            var updatedItem = await _uow.Items.GetByIdAsync(id); // Get updated item
            if (updatedItem == null) return null;

            return new ItemDto(
                updatedItem.Id,
                updatedItem.Name,
                updatedItem.Description,
                updatedItem.Price
            );
        }

        /// <summary>
        /// Deletes an item by its ID.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <returns>True if deleted, false otherwise.</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var deleted = await _uow.Items.DeleteAsync(id); // Delete item
                if (!deleted)
                {
                    await _uow.RollbackAsync();
                    return false;
                }
                await _uow.CommitAsync(); // Commit transaction
                return true;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
