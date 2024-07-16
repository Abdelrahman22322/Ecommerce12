using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services;

public class ProductTagService : IProductTagService
{
    private readonly IGenericRepository<ProductTag?> _productTagRepository;

    public ProductTagService(IGenericRepository<ProductTag?> productTagRepository)
    {
        _productTagRepository = productTagRepository;
    }

    public async Task AddAsync(ProductTag? productTag)
    {
        if (productTag == null)
        {
            throw new ArgumentNullException(nameof(productTag));
        }

        await _productTagRepository.AddAsync(productTag);
        await _productTagRepository.SaveAsync();
    }

    public async Task UpdateAsync(int productId, ProductTag? productTag)
    {
        if (productTag == null)
        {
            throw new ArgumentNullException(nameof(productTag));
        }

        var existingProductTag = await _productTagRepository.FindAsync(x => x.ProductId == productTag.ProductId && x.TagId == productTag.TagId);
        var existingProductTag1 = existingProductTag?.FirstOrDefault();

        if (existingProductTag1 != null)
        {
            existingProductTag1.ProductId = productTag.ProductId;
            existingProductTag1.TagId = productTag.TagId;

            await _productTagRepository.UpdateAsync(existingProductTag1);
            await _productTagRepository.SaveAsync();
        }
        else
        {
            throw new Exception("ProductTag not found.");
        }
    }




    public async Task DeleteAsync(int productId, int tagId)
    {
        var existingProductTag = await _productTagRepository.FindCompositeKeyAsync(x => x.ProductId == productId && x.TagId == tagId);

        if (existingProductTag != null)
        {
            await _productTagRepository.DeleteAsync(existingProductTag);
            await _productTagRepository.SaveAsync();
        }
        else
        {
            throw new Exception("ProductTag not found.");
        }
    }



    public async Task<ProductTag?> DetermineProductTagAsync(int productId, int tagId)
    {
        var existingProductTag = await _productTagRepository.FindAsync(x => x.ProductId == productId && x.TagId == tagId);
        var existingProductTag1 = existingProductTag?.FirstOrDefault();

        if (existingProductTag1 != null)
        {
            return existingProductTag1;
        }

        var newProductTag = new ProductTag
        {
            ProductId = productId,
            TagId = tagId
        };

        await _productTagRepository.AddAsync(newProductTag);
        await _productTagRepository.SaveAsync();

        return newProductTag;
    }

    public async Task<IEnumerable<ProductTag>> FindAsync(Expression<Func<ProductTag, bool>> func)
    {

        var productTag = await _productTagRepository.FindAsync(func);
        if (productTag == null)
        {
            throw new Exception("ProductTag not found.");
        }

        return productTag;

    }

    public async Task<ProductTag> FindAsync1(Expression<Func<ProductTag, bool>?> predicate)
    {

         return await _productTagRepository.FindAsync1(predicate);
    }
}