using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Services;

namespace Ecommerce.Core.ServicesContracts;

public interface IProductTagService
{
    Task AddAsync(ProductTag? productTag);
    Task UpdateAsync(int productId ,ProductTag? productTag);
    Task DeleteAsync(int productid,int productTagId);


    Task<ProductTag?> DetermineProductTagAsync(int newProductProductId, int tagEntityId);


    Task<IEnumerable<ProductTag>> FindAsync(Expression<Func<ProductTag, bool>> func);
      Task<ProductTag> FindAsync1(Expression<Func<ProductTag, bool>?> predicate);
}