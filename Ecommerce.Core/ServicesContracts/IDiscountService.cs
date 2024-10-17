using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IDiscountService
{
    Task<IEnumerable<Discount?>> GetAllAsync();
    Task<DiscountDTO> GetByIdAsync(int id);
    Task AddAsync(DiscountDTO discount);
    Task UpdateAsync(int id,DiscountDTO discount);
    Task DeleteAsync(int id);

    Task<IEnumerable<Discount?>> FindAsync(Expression<Func<Discount?, bool>> func);
    Task RemoveProductFromDiscountAsync(int discountId, int productId);
    Task AddDiscountByCategoryAsync(string category, DiscountByDTO discount);
    Task AddDiscountByBrandAsync(string brand, DiscountByDTO discount);

    Task<IEnumerable<Discount?>> GetDiscountsByBrandAsync(string brandName);
    Task<IEnumerable<Discount?>> GetDiscountsByCategoryAsync(string category);
    Task<IEnumerable<Discount?>> GetDiscountsByNameAsync(string discountName);
}