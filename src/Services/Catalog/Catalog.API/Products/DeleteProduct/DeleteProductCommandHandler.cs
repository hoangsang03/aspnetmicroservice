using FluentValidation;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IQuery<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage("Product ID is required");
    }
}

internal class DeleteProductCommandHandler(IDocumentSession session) : IQueryHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(request.Id); // we will use another approach
        }

        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);

        DeleteProductResult result = new(true);
        return result;
    }
}
