using Catalog.API.Products.GetProducts;

namespace Catalog.API.Common.Mapping
{
    public class ProductMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GetProductsRequest, GetProductsQuery>();
        }
    }
}
