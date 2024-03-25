namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<ProductDto> Products, int Total);

internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Method} - Begin -  with {@Query}", nameof(Handle), query);
        IReadOnlyList<Product> products = await session.Query<Product>().ToListAsync(cancellationToken);
        List<ProductDto> productDtos = products.Adapt<List<ProductDto>>();

        var result = new GetProductsResult(productDtos, productDtos.Count);
        logger.LogInformation("{Method} - End - Result {@Result}", nameof(Handle), result);
        return result;
    }
}
