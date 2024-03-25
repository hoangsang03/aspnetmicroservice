namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryRequest(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResponse(IEnumerable<ProductDto> Products, int Total);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //GET "/products/category/{category}"
        app.MapGet("/products/category/{category}", async ([AsParameters] GetProductByCategoryRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductByCategoryQuery>();
            GetProductByCategoryResult result = await sender.Send(query);
            var response = result.Adapt<GetProductByCategoryResponse>();

            return Results.Ok(response);
        }).WithTags(Constants.TAG_PRODUCT)
        .WithName("GetProductByCategory")
        .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products API")
        .WithDescription("Get Products According To The Given Category")
        .WithOpenApi();
    }
}
