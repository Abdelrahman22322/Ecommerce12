using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services;

public class ProductAttributeValueServices : IProductAttributeValueServices
{  
    private readonly IGenericRepository<ProductAttributeValue> _productAttributeValueRepository;

    public ProductAttributeValueServices(IGenericRepository<ProductAttributeValue> productAttributeValueRepository)
    {
        _productAttributeValueRepository = productAttributeValueRepository;
    }
    public async Task AddProductAttributeValues(ProductAttributeValue entity)
    {
        await _productAttributeValueRepository.AddAsync(entity);
    }
    public async Task<bool> UpdateAsync(ProductAttributeValue entity)
    {
        _productAttributeValueRepository.UpdateAsync(entity);
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _productAttributeValueRepository.GetByIdAsync(id);
        if (entity == null)
            return false;
        await _productAttributeValueRepository.DeleteAsync(entity);
        return true;
    }
    public async Task<bool> DeleteRange(IEnumerable<ProductAttributeValue> entities)
    {
        await _productAttributeValueRepository.DeleteRange(entities);
        return true;
    }
    public async Task<ProductAttributeValue> GetByIdAsync(int id)
    {
        return await _productAttributeValueRepository.GetByIdAsync(id);
    }
    public async Task<IEnumerable<ProductAttributeValue>> GetAllAsync(Expression<Func<ProductAttributeValue, bool>>? predicate, string? includeword)
    {
        return await _productAttributeValueRepository.GetAllAsync(predicate, includeword);
    }
    public async Task SaveAsync()
    {
        await _productAttributeValueRepository.SaveAsync();
    }
}
    
