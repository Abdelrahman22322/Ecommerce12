using System.Security.Cryptography.X509Certificates;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.Infrastructure.Repository;

public class ProductRepository :  IProductRepository  
{

     private readonly ApplicationDbContext _context;
    

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
      
    }


    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(ProductFilter filter)
    {

        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(p => p.ProductName.Contains(filter.Name));
        }

        if (!string.IsNullOrEmpty(filter.Category))
        {
            query = query.Where(p => p.Category.Name == filter.Category);
        }

        if (!string.IsNullOrEmpty(filter.Brand))
        {
            query = query.Where(p => p.Brand.Name == filter.Brand);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.UnitPrice >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.UnitPrice <= filter.MaxPrice.Value);
        }

        if (filter.MinDiscount.HasValue)
        {
            query = query.Where(p => p.Discounts.DiscountAmount >=  filter.MinDiscount);
        }

        if (filter.MaxDiscount.HasValue)
        {
            query = query.Where(p => p.Discounts.DiscountAmount <= filter.MaxDiscount.Value);
        }

        if (filter.MinRating.HasValue)
        {
            query = query.Where(p => p.Ratings.Select( r => r.Value).Average() >= filter.MinRating.Value);
        }

        if (filter.MaxRating.HasValue)
        {
            query = query.Where(p => p.Ratings.Select(r=> r.Value).Average() <= filter.MaxRating.Value);
        }

        return await query.ToListAsync();

    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {

        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductAttributeValues)
            .Where(p => p.ProductName.Contains(searchTerm)
                        || p.Brand.Name.Contains(searchTerm)
                        || p.Category.Name.Contains(searchTerm)
                        || p.ProductAttributeValues.Any(a => a.Value.Contains(searchTerm)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetTopSellingProductsAsync()
    {


        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .OrderByDescending(p => p.OrderDetails.Count  )
            .Take(10)
            .ToListAsync();


         



    }

    public async Task<IEnumerable<Product>> GetNewestProductsAsync()
    {




        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .OrderByDescending(p => p.CreatedDate)
            .Take(10)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
    {

        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.IsFeatured)
            .Take(10)
            .ToListAsync();

    }

    public async Task<IEnumerable<Product>> GetProductsOnSaleAsync()
    {

        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Discounts)
            .Where(p => p.Discounts.DiscountAmount > 0)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId)
    {

        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.SupplierId == supplierId)
            .ToListAsync();

    }

    public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
    {

        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.UnitsInStock == 0)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetRecommendedProductsAsync()
    {
                
        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.Ratings.Select(r => r.Value).Average() >= 4)
            .ToListAsync();

    }

    public async Task<IEnumerable<Product>> GetRelatedProductsAsync(int productId)
    {

        var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.CategoryId == product.CategoryId)
            .ToListAsync();


    }

    public async Task<IEnumerable<Product>> GetSimilarProductsAsync(int productId)
    {

        var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.BrandId == product.BrandId)
            .ToListAsync();

    }
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}