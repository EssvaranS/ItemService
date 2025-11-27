using ItemService.Api.Responses;
using ItemService.Application.DTOs;
using ItemService.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net; 

namespace ItemService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        public async Task<ApiResponse<ItemDto>> Create([FromBody] CreateItemDto dto)
        {
            if (!ModelState.IsValid)
                return ApiResponse<ItemDto>.Fail("Validation failed", (int)HttpStatusCode.BadRequest);
            var created = await _itemService.CreateAsync(dto);
            return ApiResponse<ItemDto>.Ok(created, statusCode: (int)HttpStatusCode.Created);
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<ItemDto>>> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return ApiResponse<IEnumerable<ItemDto>>.Ok(items, statusCode: (int)HttpStatusCode.OK);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<ItemDto>> GetById(string id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return ApiResponse<ItemDto>.Fail("Item not found", (int)HttpStatusCode.NotFound);
            return ApiResponse<ItemDto>.Ok(item, statusCode: (int)HttpStatusCode.OK);
        }

        [HttpPatch("{id}")]
        public async Task<ApiResponse<ItemDto>> Update(string id, [FromBody] UpdateItemDto dto)
        {
            if (!ModelState.IsValid)
                return ApiResponse<ItemDto>.Fail("Validation failed", (int)HttpStatusCode.BadRequest);
            var updated = await _itemService.UpdateAsync(id, dto);
            if (updated == null) return ApiResponse<ItemDto>.Fail("Item not found", (int)HttpStatusCode.NotFound);
            return ApiResponse<ItemDto>.Ok(updated, statusCode: (int)HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> Delete(string id)
        {
            var deleted = await _itemService.DeleteAsync(id);
            if (!deleted) return ApiResponse<string>.Fail("Item not found", (int)HttpStatusCode.NotFound);
            return ApiResponse<string>.Ok("", "Item deleted", (int)HttpStatusCode.OK);
        }
    }
}