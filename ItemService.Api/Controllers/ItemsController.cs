using ItemService.Api.Responses;
using ItemService.Application.DTOs;
using ItemService.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemService.Api.Controllers
{
    /// <summary>
    /// Controller for managing items in the ItemService API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="itemService">Service for item operations.</param>
        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="dto">Item creation data.</param>
        /// <returns>Created item response.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
        {
            var created = await _itemService.CreateAsync(dto); // Create item
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<ItemDto>.Ok(created));
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>List of items.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllAsync(); // Retrieve all items
            return Ok(ApiResponse<IEnumerable<ItemDto>>.Ok(items));
        }

        /// <summary>
        /// Gets an item by its ID.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <returns>Item response or not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _itemService.GetByIdAsync(id); // Retrieve item by ID
            if (item == null) return NotFound(ApiResponse<string>.Fail("Item not found"));
            return Ok(ApiResponse<ItemDto>.Ok(item));
        }

        /// <summary>
        /// Updates an item by its ID.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <param name="dto">Update data.</param>
        /// <returns>Updated item response or not found.</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateItemDto dto)
        {
            var updated = await _itemService.UpdateAsync(id, dto); // Update item
            if (updated == null) return NotFound(ApiResponse<string>.Fail("Item not found"));
            return Ok(ApiResponse<ItemDto>.Ok(updated));
        }

        /// <summary>
        /// Deletes an item by its ID.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <returns>Delete result response.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _itemService.DeleteAsync(id); // Delete item
            if (!deleted) return NotFound(ApiResponse<string>.Fail("Item not found"));
            return Ok(ApiResponse<string>.Ok("", "Item deleted"));
        }
    }
}
