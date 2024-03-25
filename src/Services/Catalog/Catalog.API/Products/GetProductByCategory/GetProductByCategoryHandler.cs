namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<ProductDto> Products, int Total);
public class GetProductByCategoryHandler(IDocumentSession session, ILogger<GetProductByCategoryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Method} - Begin - query {@Query}", nameof(Handle), query);

        IReadOnlyList<Product> products = await session.Query<Product>()
            .Where(p => p.Category != null
                        && p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);

        List<ProductDto> productDtos = products.Adapt<List<ProductDto>>();
        GetProductByCategoryResult result = new(productDtos, productDtos.Count);
        return result;
    }
}

