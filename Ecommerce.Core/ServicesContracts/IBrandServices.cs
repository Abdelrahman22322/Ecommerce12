using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using System.Linq.Expressions;

namespace Ecommerce.Core.ServicesContracts;

public interface IBrandServices 
{

    Task AddBrand(Brand entity);
    
    Task<bool> UpdateAsync(Brand entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteRange(IEnumerable<Brand> entities);
    Task<Brand> GetByIdAsync(int id);
    Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>>? predicate, string? includeword);
   
    Task SaveAsync();


}