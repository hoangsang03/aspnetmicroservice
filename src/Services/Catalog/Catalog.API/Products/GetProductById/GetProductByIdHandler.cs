namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);

public class GetProductByIdHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product? product = await session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(request.Id); // it should be replaced
        }

        GetProductByIdResult result = new(product.Adapt<ProductDto>());
        return result;
    }
}
