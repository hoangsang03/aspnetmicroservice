namespace Catalog.API.Products.GetProductById;

public record GetProductByIdRequest(Guid Id);
public record GetProductByIdResponse(ProductDto Product);
public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //GET "/products/id"
        app.MapGet("/products/{id:guid}", async ([AsParameters] GetProductByIdRequest request, ISender sender) =>
        {
            GetProductByIdQuery query = request.Adapt<GetProductByIdQuery>();

            try
            {
                GetProductByIdResult result = await sender.Send(query);
                GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(response);
            }
            catch (ProductNotFoundException) // we will use another approach later
            {
                return Results.NotFound();
            }
        }).WithTags(Constants.TAG_PRODUCT)
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product By ID API")
        .WithDescription("Get Product With The Given ID")
        .WithOpenApi();
    }

}
