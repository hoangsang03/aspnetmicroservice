namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("{Method} - Begin - command {@Command}", nameof(Handle), command);

            bool isExist = await session.Query<Product>().AnyAsync(p => p.Id == command.Id, cancellationToken);

            if (!isExist)
            {
                throw new ProductNotFoundException(command.Id);
            }

            Product product = command.Adapt<Product>();

            session.Update<Product>(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
