namespace Catalog.API.Common.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base("Product Not Found") { }

        public ProductNotFoundException(Guid Id) : base($"Product {Id} Not Found") { }
    }
}
