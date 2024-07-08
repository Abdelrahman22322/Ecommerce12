using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Linq.Expressions;

public class CategoryService : ICategoriesServices
{
    private readonly IGenericRepository<Category> _categoryRepository;

    public CategoryService(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task AddCategory(Category entity)
    {
        if (string.IsNullOrEmpty(entity.Name))
        {
            throw new ArgumentNullException("category name required");
        }

        await _categoryRepository.AddAsync(entity);
        await _categoryRepository.SaveAsync();
    }

    public async Task<bool> UpdateAsync(Category entity)
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

    public async Task<bool> DeleteRange(IEnumerable<Category> entities)
    {
        await _categoryRepository.DeleteRange(entities);
        await _categoryRepository.SaveAsync();
        return true;
    }

    public Task<Category> GetByIdAsync(int id)
    {
        return _categoryRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? predicate, string? includeword)
    {
        return _categoryRepository.GetAllAsync(predicate, includeword);
    }

    public async Task SaveAsync()
    {
        await _categoryRepository.SaveAsync();
    }
}