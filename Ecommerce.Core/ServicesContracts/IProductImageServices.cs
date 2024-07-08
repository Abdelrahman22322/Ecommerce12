using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductImageServices
{
    Task AddProductImage(ProductImage entity);
    Task<bool> UpdateAsync(ProductImage entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRange(IEnumerable<ProductImage> entities);
    Task<ProductImage> GetByIdAsync(int id);
    Task<IEnumerable<ProductImage>> GetAllAsync(Expression<Func<ProductImage, bool>>? predicate, string? includeword);
    Task SaveAsync();
}