using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Ecommerce.Core.ServicesContracts;

public interface IBrandServices 
{

    Task AddBrand(CreateBrandDto dto);
    Task<bool> UpdateAsync(UpdateBrandDto dto);
    Task<bool> DeleteAsync(int id);
    Task<BrandDto> GetByIdAsync(int id);
 //   Task<IEnumerable<BrandDto>> GetAllAsync(Expression<Func<Brand, bool>>? predicate = null);
    Task<Brand?> DetermineBrandAsync(string brandName);
    Task<IEnumerable<BrandDto>> GetAllAsync();




}