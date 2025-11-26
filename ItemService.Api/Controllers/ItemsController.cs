using ItemService.Api.Responses;
using ItemService.Application.DTOs;
using ItemService.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
        {
            var created = await _itemService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<ItemDto>.Ok(created));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ItemDto>>.Ok(items));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound(ApiResponse<string>.Fail("Item not found"));
            return Ok(ApiResponse<ItemDto>.Ok(item));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateItemDto dto)
        {
            var updated = await _itemService.UpdateAsync(id, dto);
            if (updated == null) return NotFound(ApiResponse<string>.Fail("Item not found"));
            return Ok(ApiResponse<ItemDto>.Ok(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _itemService.DeleteAsync(id);
            if (!deleted) return NotFound(ApiResponse<string>.Fail("Item not found"));
            return Ok(ApiResponse<string>.Ok("", "Item deleted"));
        }
    }
}
