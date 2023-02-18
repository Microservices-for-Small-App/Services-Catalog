using System.ComponentModel.DataAnnotations;

namespace Catalog.Data.Dtos;

public record CreateCatalogItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);