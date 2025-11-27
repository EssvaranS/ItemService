using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ItemService.Application.DTOs
{
    
    /// <summary>
    /// Data transfer object for item details.
    /// </summary>
    /// <param name="Id">Item ID.</param>
    /// <param name="Name">Item name.</param>
    /// <param name="Description">Item description.</param>
    /// <param name="Price">Item price.</param>
    public record ItemDto(string? Id, string Name, string Description, decimal Price);

    /// <summary>
    /// Data transfer object for creating a new item.
    /// </summary>
    /// <param name="Name">Item name.</param>
    /// <param name="Description">Item description.</param>
    /// <param name="Price">Item price.</param>
    public record CreateItemDto(
        [Required]
        [StringLength(100, MinimumLength = 2)]
        string Name,
        [StringLength(500)]
        string Description,
        [Range(0.01, 1000000)]
        decimal Price
    );

    /// <summary>
    /// Data transfer object for updating an item.
    /// </summary>
    /// <param name="Name">Item name (optional).</param>
    /// <param name="Description">Item description (optional).</param>
    /// <param name="Price">Item price (optional).</param>
    public record UpdateItemDto(
        [StringLength(100, MinimumLength = 2)]
        string? Name,
        [StringLength(500)]
        string? Description,
        [Range(0.01, 1000000)]
        decimal? Price
    );

}
