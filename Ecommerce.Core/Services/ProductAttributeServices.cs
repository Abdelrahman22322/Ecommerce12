using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services;

public class ProductAttributeServices : IProductAttributeServices
{
    private readonly IGenericRepository<ProductAttribute> _productAttributeRepository;

    public ProductAttributeServices(IGenericRepository<ProductAttribute> productAttributeRepository)
    {
        _productAttributeRepository = productAttributeRepository;
    }

    public async Task AddProductAttribute(ProductAttribute entity)
    {
        await _productAttributeRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateAsync(ProductAttribute entity)
    {
        _productAttributeRepository.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _productAttributeRepository.GetByIdAsync(id);
        if (entity == null)
            return false;

        await _productAttributeRepository.DeleteAsync(entity);
        return true;
    }

    public async Task<bool> DeleteRange(IEnumerable<ProductAttribute> entities)
    {
        await _productAttributeRepository.DeleteRange(entities);
        return true;
    }

    public async Task<ProductAttribute> GetByIdAsync(int id)
    {
        return await _productAttributeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ProductAttribute>> GetAllAsync(Expression<Func<ProductAttribute, bool>>? predicate, string? includeword)
    {
        return await _productAttributeRepository.GetAllAsync(predicate, includeword);
    }

    public async Task SaveAsync()
    {
        await _productAttributeRepository.SaveAsync();
    }


}
