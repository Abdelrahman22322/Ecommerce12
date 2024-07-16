using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductAttributeServices
{
    Task AddProductAttribute(ProductAttribute? entity);
    Task<ProductAttribute?> DetermineAttributeAsync(string attributeName);
    Task<bool> UpdateAsync(ProductAttribute? entity);
    Task<bool> DeleteAsync(int id);

    Task<ProductAttribute?> GetByIdAsync( int  id);
    Task<IEnumerable<ProductAttribute?>> FindAsync(Expression<Func<ProductAttribute?, bool>?> func);
    Task<ProductAttribute> GetByNameAsync(string attributeName);
    Task<ProductAttribute?> FindAsync1(Expression<Func<ProductAttribute?, bool>?> func);
}