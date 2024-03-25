using Catalog.API.Products.GetProducts;

namespace Catalog.API.Common.Mappings;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetProductsRequest, GetProductsQuery>();
    }
}
