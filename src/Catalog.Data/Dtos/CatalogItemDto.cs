namespace Catalog.Data.Dtos;

public record CatalogItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);