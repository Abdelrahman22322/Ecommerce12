using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductAttributeValueServices
{
    Task AddProductAttributeValues(ProductAttributeValue entity);
    Task<bool> UpdateAsync(ProductAttributeValue entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRange(IEnumerable<ProductAttributeValue> entities);
    Task<ProductAttributeValue> GetByIdAsync(int id);
    Task<IEnumerable<ProductAttributeValue>> GetAllAsync(Expression<Func<ProductAttributeValue, bool>>? predicate, string? includeword);
    Task SaveAsync();
}