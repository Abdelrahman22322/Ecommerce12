using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface ICategoriesServices
{
    Task AddCategory(Category entity);
    Task<bool> UpdateAsync(Category entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRange(IEnumerable<Category> entities);
    Task<Category> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? predicate, string? includeword);
    Task SaveAsync();
}