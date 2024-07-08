using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductAttributeServices
{
    Task AddProductAttribute(ProductAttribute entity);
    Task<bool> UpdateAsync(ProductAttribute entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRange(IEnumerable<ProductAttribute> entities);
    Task<ProductAttribute> GetByIdAsync(int id);
    Task<IEnumerable<ProductAttribute>> GetAllAsync(Expression<Func<ProductAttribute, bool>>? predicate, string? includeword);
    Task SaveAsync();
}