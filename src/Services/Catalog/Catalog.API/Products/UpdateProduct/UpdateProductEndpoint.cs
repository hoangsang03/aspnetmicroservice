namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
{
    public Guid Id { get; internal set; }
};

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //PUT "products/{id}"
        app.MapPut("/products/{id}", async (Guid Id, UpdateProductRequest request, ISender sender) =>
        {
            request.Id = Id;
            var command = request.Adapt<UpdateProductCommand>();
            UpdateProductResult result = await sender.Send(command);

            var response = result.Adapt<UpdateProductResult>();
            return Results.Ok(response);
        }).WithTags(Constants.TAG_PRODUCT)
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Product API")
        .WithDescription("Update Product With The Supplied Data")
        .WithOpenApi();
    }
}
