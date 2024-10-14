using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.DTO;
using Ecommerce.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Product> _repository;

        public ProductRepository(ApplicationDbContext context, IGenericRepository<Product> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetProductsByFilterAsync(ProductFilter filter)
        {
            var query = _context.Products.Include(p => p.ProductImages).AsQueryable();

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
                query = query.Where(p => p.Discount.DiscountAmount >= filter.MinDiscount);
            }

            if (filter.MaxDiscount.HasValue)
            {
                query = query.Where(p => p.Discount.DiscountAmount <= filter.MaxDiscount.Value);
            }

            if (filter.MinRating.HasValue)
            {
                query = query.Where(p => p.Ratings.Average(r => r.Value) >= filter.MinRating.Value);
            }

            if (filter.MaxRating.HasValue)
            {
                query = query.Where(p => p.Ratings.Average(r => r.Value) <= filter.MaxRating.Value);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductAttributeValues)
                .Where(p => p.ProductName.Contains(searchTerm)
                            || p.Brand.Name.Contains(searchTerm)
                            || p.Category.Name.Contains(searchTerm)
                            || p.ProductAttributeValues.Any(a => a.Value.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.OrderDetails.Count)
                .Take(10)
                .AsNoTracking()
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
                .AsNoTracking()
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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsOnSaleAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Discount)
                .Where(p => p.Discount.DiscountAmount > 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.SupplierId == supplierId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.UnitsInStock == 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetRecommendedProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.Ratings.Average(r => r.Value) >= 4)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetSimilarOrRelatedProductsAsync(int productId, bool related = false)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => related ? p.CategoryId == product.CategoryId : p.BrandId == product.BrandId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProducCardtDTO>> GetProductCards()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Select(p => new ProducCardtDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    ImageUrl = p.ProductImages.FirstOrDefault().ImageUrl
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDetailDTO>> GetProducts()
        {

            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Select(p => new ProductDetailDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    ImageUrl = p.ProductImages.FirstOrDefault()!.ImageUrl,
                    Brand = p.Brand.Name,
                    Category = p.Category.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductAttributeValues).ThenInclude(pav => pav.ProductAttribute)
                .Include(p => p.ProductImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);
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
}
