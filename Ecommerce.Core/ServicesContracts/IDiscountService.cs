using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IDiscountService
{
    Task<IEnumerable<Discount>> GetAllAsync();
    Task<Discount> GetByIdAsync(int id);
    Task AddAsync(Discount discount);
    Task UpdateAsync(int id,Discount discount);
    Task DeleteAsync(int id);

    Task<IEnumerable<Discount?>> FindAsync(Expression<Func<Discount, bool>?> func);
}