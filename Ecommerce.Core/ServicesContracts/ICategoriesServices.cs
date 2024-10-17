using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICategoriesServices
{
    Task AddCategory(CreateCategoryDto entity);
    public Task<Category?> DetermineCategoryAsync(string Categoryname);
    Task<bool> UpdateAsync(UpdateCategoryDto? entity);
    Task<bool> DeleteAsync(int id);
   // Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> func);
   Task<CategoryDto?> GetByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetAllAsync();
   
}