using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.DTO;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.Core.Domain.RepositoryContracts;

/// <summary>
/// Represents a repository for managing products.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves products based on the specified filter.
    /// </summary>
    /// <param name="filter">The filter to apply.</param>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetProductsByFilterAsync(ProductFilter filter);

    /// <summary>
    /// Searches for products based on the specified search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match.</param>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);

    /// <summary>
    /// Retrieves the top selling products.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetTopSellingProductsAsync();

    /// <summary>
    /// Retrieves the newest products.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetNewestProductsAsync();

    /// <summary>
    /// Retrieves the featured products.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetFeaturedProductsAsync();

    /// <summary>
    /// Retrieves the products on sale.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetProductsOnSaleAsync();

    /// <summary>
    /// Retrieves the products supplied by the specified supplier.
    /// </summary>
    /// <param name="supplierId">The ID of the supplier.</param>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId);

    /// <summary>
    /// Retrieves the out of stock products.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetOutOfStockProductsAsync();

    /// <summary>
    /// Retrieves the recommended products.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetRecommendedProductsAsync();

     Task<IEnumerable<Product>> GetSimilarOrRelatedProductsAsync(int productId, bool related);

     Task<IEnumerable<ProducCardtDTO>> GetProductCards();
     Task<IEnumerable<ProductDetailDTO>> GetProducts();
    Task<Product> GetByIdAsync(int id);

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
