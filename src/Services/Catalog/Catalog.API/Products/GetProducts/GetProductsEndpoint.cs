namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest();
public record GetProductsResponse(IEnumerable<ProductDto> Products, int Total);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // URI: "/products"
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();
            GetProductsResult result = await sender.Send(query);

            GetProductsResponse getProductResponse = result.Adapt<GetProductsResponse>();

            return Results.Ok(getProductResponse);
        })
        .WithTags(Constants.TAG_PRODUCT)
        .WithName("GetProduct")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get All Products API")
        .WithDescription("Get All Products Without Condition")
        .WithOpenApi();
    }
}
