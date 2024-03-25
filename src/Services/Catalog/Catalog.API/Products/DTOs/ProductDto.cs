namespace Catalog.API.Products.DTOs;

public record ProductDto(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
