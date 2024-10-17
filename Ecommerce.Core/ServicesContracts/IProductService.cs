using System.Collections;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.DTO;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductService

{
    public Task AddAsync(AddProductDto product);
    public Task UpdateAsync(int productId, UpdateProductDto productDto);
    public Task DeleteAsync(int productId);

    Task<IEnumerable<ProducCardtDTO>> GetProductsByFilterAsync(ProductFilter filter);


    Task<IEnumerable<ProducCardtDTO>> SearchProductsAsync(string searchTerm);

    Task<IEnumerable<ProducCardtDTO>> GetTopSellingProductsAsync();

    Task<IEnumerable<ProducCardtDTO>> GetNewestProductsAsync();

    Task<IEnumerable<ProducCardtDTO>> GetFeaturedProductsAsync();

    Task<IEnumerable<ProducCardtDTO>> GetProductsOnSaleAsync();

    Task<IEnumerable<ProducCardtDTO>> GetProductsBySupplierAsync(int supplierId);

    Task<IEnumerable<ProducCardtDTO>> GetOutOfStockProductsAsync();

    Task<IEnumerable<ProductDetailDTO>> GetProducts();
    Task<IEnumerable<ProducCardtDTO>> GetProductCards();


    Task<ProductDetailDTO> ArchiveProduct(int productId);
    Task<ProductDetailDTO> UnArchiveProduct(int productId);
    Task<UpdateProductDto> GetProductForUpdate(int productId);
    Task<IEnumerable<ProducCardtDTO>> GetRecommendedProductsAsync();
    Task<IEnumerable<ProducCardtDTO>> GetSimilarProductsOrRelatedAsync(int productId);


    Task<IEnumerable<ProducCardtDTO>> GetProductCardsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProducCardtDTO>> GetProductCardsByBrandAsync(int brandId);
    Task<IEnumerable<ProducCardtDTO>> GetProductCardsByDiscountIdAsync(int discountId);
    public Task<byte[]> ExportPdfAsync();

}