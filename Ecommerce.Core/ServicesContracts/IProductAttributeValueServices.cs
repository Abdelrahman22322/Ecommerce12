using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductAttributeValueServices
{
    Task AddProductAttributeValues(ProductAttributeValue? entity);
    Task<ProductAttributeValue?> DetermineAttributeValueAsync(int attributeId, string value);
    Task<bool> UpdateAsync(ProductAttributeValue? entity);
    Task<bool> DeleteAsync(int productid,int id);

    Task<IEnumerable<ProductAttributeValue>?> FindAsync(Expression<Func<ProductAttributeValue?, bool>> func);
     Task<ProductAttributeValue?> FindAsync1(Expression<Func<ProductAttributeValue?, bool>> predicate);
}