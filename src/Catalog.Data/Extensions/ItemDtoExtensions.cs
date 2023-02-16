using Catalog.Data.Dtos;
using Catalog.Data.Entities;

namespace Catalog.Data.Extensions;

public static class ItemDtoExtensions
{
    public static ItemDto AsDto(this CatalogItem item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }
}
