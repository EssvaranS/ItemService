using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemService.Application.DTOs
{
    
    public record ItemDto(string? Id, string Name, string Description, decimal Price);

    public record CreateItemDto(string Name, string Description, decimal Price);

    public record UpdateItemDto(string? Name, string? Description, decimal? Price);

}
