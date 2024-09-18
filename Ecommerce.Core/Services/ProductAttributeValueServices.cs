using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using FluentValidation;

namespace Ecommerce.Core.Services;

public class ProductAttributeValueServices : IProductAttributeValueServices
{  
    private readonly IGenericRepository<ProductAttributeValue?> _productAttributeValueRepository;
   

    public ProductAttributeValueServices(IGenericRepository<ProductAttributeValue?> productAttributeValueRepository)
    {
        _productAttributeValueRepository = productAttributeValueRepository;
    }
    public async Task AddProductAttributeValues(ProductAttributeValue? entity)
    {
        await _productAttributeValueRepository.AddAsync(entity);
    }

    

   public async Task<bool> UpdateAsync(ProductAttributeValue? entity)
    {
        if (entity == null)
            return false;



        if (entity == null)
        {
            return false;
        }

        // Validate the entity
        if (entity.Id <= 0)
        {
            throw new ArgumentException("Invalid entity ID.");
        }

        if (string.IsNullOrWhiteSpace(entity.Value))
        {
            throw new ValidationException("Value is required.");
        }

        try
        {
            await _productAttributeValueRepository.UpdateAsync(entity);
            await _productAttributeValueRepository.SaveAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not shown here)
            throw new Exception("An error occurred while updating the entity.", ex);
        }
    }
    public async Task<bool> DeleteAsync(int productid, int id)
    {


        var entity = await _productAttributeValueRepository.FindAsync1(x => x.ProductAttributeId == productid && x.Id == id, null);
        if (entity == null)
            return false;

        await _productAttributeValueRepository.DeleteAsync(entity);
        await _productAttributeValueRepository.SaveAsync();

        return true;
    }

    public async Task<IEnumerable<ProductAttributeValue>?> FindAsync(Expression<Func<ProductAttributeValue?, bool>> func )
    {

        return await _productAttributeValueRepository.FindAsync(func, null);
    }

    public async Task<ProductAttributeValue?> FindAsync1(Expression<Func<ProductAttributeValue?, bool>> predicate)
    {
       return await _productAttributeValueRepository.FindAsync1(predicate, null);
    }

    public async Task<ProductAttributeValue?> DetermineAttributeValueAsync(int attributeId, string value)
    {
        var existingAttributeValues = await _productAttributeValueRepository.FindAsync(x => x.Id == attributeId && x.Value== value, null);
        var existingAttributeValue = existingAttributeValues?.FirstOrDefault();
        if (existingAttributeValue == null)
        {
            var attributeValue = new ProductAttributeValue
            {
                ProductAttributeId = attributeId,
                Value = value
            };
            await _productAttributeValueRepository.AddAsync(attributeValue);
            await _productAttributeValueRepository.SaveAsync();
            return attributeValue;
        }

        return existingAttributeValue;
    }


}