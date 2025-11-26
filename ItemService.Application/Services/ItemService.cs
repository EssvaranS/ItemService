using ItemService.Api.Responses;
using ItemService.Application.DTOs;
using ItemService.Application.IServices;
using ItemService.Domain.Entities;
using ItemService.Infrastructure.UnitOfWork;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemService.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _uow;

        public ItemService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ItemDto> CreateAsync(CreateItemDto dto)
        {
            var entity = new Item { Name = dto.Name, Description = dto.Description, Price = dto.Price };
            await _uow.Items.AddAsync(entity);
            await _uow.CommitAsync();
            return new ItemDto(entity.Id, entity.Name, entity.Description, entity.Price);
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _uow.Items.GetAllAsync();
            return items.Select(i => new ItemDto(i.Id, i.Name, i.Description, i.Price));
        }

        public async Task<ItemDto?> GetByIdAsync(string id)
        {
            var item = await _uow.Items.GetByIdAsync(id);
            if (item == null) return null;
            return new ItemDto(item.Id, item.Name, item.Description, item.Price);
        }

        public async Task<ItemDto?> UpdateAsync(string id, UpdateItemDto dto)
        {
            var updates = new List<UpdateDefinition<Item>>();

            var properties = typeof(UpdateItemDto).GetProperties();

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

            if (!updates.Any())
                return null; // No values provided ⇒ Not updated

            var updateDefinition = Builders<Item>.Update.Combine(updates);

            var result = await _uow.Items.PatchAsync(id, updateDefinition);

            if (!result)
                return null; // Item not found case

            var updatedItem = await _uow.Items.GetByIdAsync(id);

            if (updatedItem == null) return null;

            return new ItemDto(
                updatedItem.Id,
                updatedItem.Name,
                updatedItem.Description,
                updatedItem.Price
            );
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var deleted = await _uow.Items.DeleteAsync(id);
            if (!deleted) return false;
            await _uow.CommitAsync();
            return true;
        }
    }
}
