namespace Catalog.API.Dtos;

public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);