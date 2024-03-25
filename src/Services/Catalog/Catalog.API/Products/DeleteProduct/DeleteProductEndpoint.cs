namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //DELETE:  "/products/id"
        app.MapDelete("/products/{id:guid}", async ([AsParameters] DeleteProductRequest request, ISender sender) =>
        {
            DeleteProductCommand command = request.Adapt<DeleteProductCommand>();

            try
            {
                DeleteProductResult result = await sender.Send(command);
                DeleteProductResponse response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            }
            catch (ProductNotFoundException)
            {
                return Results.NotFound();
            }
        }).WithTags(Constants.TAG_PRODUCT)
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product API")
        .WithDescription("Delete A Product With The Given ID")
        .WithOpenApi();
    }

}
