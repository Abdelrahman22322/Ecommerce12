using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductService :IGenericRepository<Product>

{

    Task<IEnumerable<Product>> GetProductsByFilterAsync(ProductFilter filter);

    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);

    Task<IEnumerable<Product>> GetTopSellingProductsAsync();

    Task<IEnumerable<Product>> GetNewestProductsAsync();

    Task<IEnumerable<Product>> GetFeaturedProductsAsync();

    Task<IEnumerable<Product>> GetProductsOnSaleAsync();

    Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId);

    Task<IEnumerable<Product>> GetOutOfStockProductsAsync();

}