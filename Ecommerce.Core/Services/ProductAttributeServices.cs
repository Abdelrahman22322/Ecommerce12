using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services;

public class ProductAttributeServices : IProductAttributeServices
{
    private readonly IGenericRepository<ProductAttribute?> _productAttributeRepository;

    public ProductAttributeServices(IGenericRepository<ProductAttribute?> productAttributeRepository)
    {
        _productAttributeRepository = productAttributeRepository;
    }

    public async Task AddProductAttribute(ProductAttribute? entity)
    {
        await _productAttributeRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateAsync(ProductAttribute? entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var existingEntity = await _productAttributeRepository.GetByIdAsync(entity.Id);
        if (existingEntity == null)
        {
            return false;
        }

        existingEntity.Name = entity.Name;
        existingEntity.ProductAttributeValues = entity.ProductAttributeValues;
     

        await _productAttributeRepository.UpdateAsync(existingEntity);
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

    public async Task<ProductAttribute?> GetByIdAsync(int id)
    {

      return  await _productAttributeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ProductAttribute?>> FindAsync(Expression<Func<ProductAttribute?, bool>> func)
    {
      return  await _productAttributeRepository.FindAsync(func, null);
    }

    public async Task<ProductAttribute> GetByNameAsync(string attributeName)
    {

        var existingAttributes = await _productAttributeRepository.FindAsync(x => x.Name == attributeName, null);
        var existingAttribute = existingAttributes?.FirstOrDefault();
        if (existingAttribute == null)
        {
            throw new Exception("Attribute not found");
        }

        return existingAttribute;
    }

    public async Task<ProductAttribute?> FindAsync1(Expression<Func<ProductAttribute?, bool>> func)
    {
         
      return  await  _productAttributeRepository.FindAsync1(func, null);
    }

    public async Task<ProductAttribute?> DetermineAttributeAsync(string attributeName)
    {
        var existingAttributes = await _productAttributeRepository.FindAsync(x => x.Name == attributeName, null);
        var existingAttribute = existingAttributes?.FirstOrDefault();
        if (existingAttribute == null)
        {
            var attribute = new ProductAttribute
            {
                Name = attributeName
            };
            await _productAttributeRepository.AddAsync(attribute);
            await _productAttributeRepository.SaveAsync();
            return attribute;
        }

        return existingAttribute;
    }



}
