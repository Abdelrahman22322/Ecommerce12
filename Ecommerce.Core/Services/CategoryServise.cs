using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Linq.Expressions;

public class CategoryService : ICategoriesServices
{
    private readonly IGenericRepository<Category?> _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(IGenericRepository<Category?> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task AddCategory(CategoryDto categoryDto)
    {
        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            throw new ArgumentNullException("category name required");
        }

        var category = _mapper.Map<Category>(categoryDto);
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveAsync();
    }

    public async Task<bool> UpdateAsync(CategoryDto categoryDto)
    {
        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            throw new ArgumentNullException("category name required");
        }

        var category = _mapper.Map<Category>(categoryDto);
        await _categoryRepository.UpdateAsync(category);
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

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return _mapper.Map<CategoryDto?>(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        IEnumerable<Category> categories = await _categoryRepository.GetAllAsync(null, null);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<IEnumerable<CategoryDto>> FindAsync(Expression<Func<Category?, bool>> func)
    {
        var categories = await _categoryRepository.FindAsync(func, null);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<Category?> DetermineCategoryAsync(string categoryName)
    {
        var existingCategories = await _categoryRepository.FindAsync(x => x.Name == categoryName, null);
        var existingCategory = existingCategories?.FirstOrDefault();
        if (existingCategory == null)
        {
            var category = new Category
            {
                Name = categoryName
            };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
            return _mapper.Map<Category?>(category);
        }

        return _mapper.Map<Category?>(existingCategory);
    }
}
