using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface ICategoriesServices
{
    Task AddCategory(Category? entity);
    public Task<Category?> DetermineCategoryAsync(string Categoryname);
    Task<bool> UpdateAsync(Category? entity);
    Task<bool> DeleteAsync(int id);
   
}