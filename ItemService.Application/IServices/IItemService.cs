using ItemService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemService.Application.IServices
{
    public interface IItemService
    {
        Task<ItemDto> CreateAsync(CreateItemDto dto);
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<ItemDto?> GetByIdAsync(string id);
        Task<ItemDto?> UpdateAsync(string id, UpdateItemDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
