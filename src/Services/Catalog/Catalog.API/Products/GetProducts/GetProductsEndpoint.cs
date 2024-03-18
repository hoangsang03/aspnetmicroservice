namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest();
    public record GetProductsResponse(IEnumerable<ProductDto> ProductDtos);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();
                GetProductsResult result = await sender.Send(query);

                GetProductsResponse getProductResponse = result.Adapt<GetProductsResponse>();

                return Results.Ok(getProductResponse);
            })
            .WithTags("Product")
            .WithName("GetProduct")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product")
            .WithDescription("Get Product")
            .WithOpenApi();
        }
    }
}
