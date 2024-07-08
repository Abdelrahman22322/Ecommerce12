using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using FluentValidation;

namespace Ecommerce.Core.Services;

public class ProductServices : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IValidator<Product> _productValidator;

    public ProductServices( IGenericRepository<Product> productRepository, IValidator<Product> productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public async Task AddAsync(Product entity)
    {




    }

    public async Task<Product> AddRangeAsync(Product entities)
    {
        throw new NotImplementedException();

    }

    public async Task<Product> UpdateAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> DeleteAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> DeleteRange(IEnumerable<Product> entities)
    { 
        


    }

    public async Task<Product> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? predicate, string? includeword)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, string? includeword)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(ProductFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetTopSellingProductsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetNewestProductsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetProductsOnSaleAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
    {
        throw new NotImplementedException();
    }
}