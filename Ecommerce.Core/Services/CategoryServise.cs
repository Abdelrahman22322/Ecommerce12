using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Linq.Expressions;

public class CategoryService : ICategoriesServices
{
    private readonly IGenericRepository<Category?> _categoryRepository;

    public CategoryService(IGenericRepository<Category?> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task AddCategory(Category? entity)
    {
        if (string.IsNullOrEmpty(entity.Name))
        {
            throw new ArgumentNullException("category name required");
        }

        await _categoryRepository.AddAsync(entity);
        await _categoryRepository.SaveAsync();
    }

   

    public async Task<bool> UpdateAsync(Category? entity)
    {
        if (string.IsNullOrEmpty(entity.Name))
        {
            throw new ArgumentNullException("category name required");
        }
          


        await _categoryRepository.UpdateAsync(entity);
        await _categoryRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        await _categoryRepository.DeleteAsync(category);
        await _categoryRepository.SaveAsync();

        return true;
    }

    public async Task<Category?> DetermineCategoryAsync(string categoryName)
    {
        var existingCategories = await _categoryRepository.FindAsync(x => x.Name == categoryName);
        var existingCategory = existingCategories?.FirstOrDefault();
        if (existingCategory == null)
        {
            var category = new Category
            {
                Name = categoryName
            };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
            return category;
        }

        return existingCategory;
    }
}