
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery() : IQuery<GetProductsResult>;

    public record GetProductsResult(IEnumerable<ProductDto> ProductDtos);

    public record ProductDto(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

    public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("");
            IReadOnlyList<Product> products = await session.Query<Product>().ToListAsync(cancellationToken);
            List<ProductDto> productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductsResult(productDtos);
        }
    }
}
