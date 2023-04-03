using Catalog.Data.Dtos;
using Catalog.Data.Entities;

namespace Catalog.Data.Extensions;

public static class CatalogItemDtoExtensions
{
    public static CatalogItemDto AsDto(this CatalogItem item)
    {
        return new CatalogItemDto(item.Id, item.Name!, item.Description!, item.Price, item.CreatedDate);
    }
}
