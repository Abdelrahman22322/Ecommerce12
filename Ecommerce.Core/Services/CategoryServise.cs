using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services
{
    public class CategoryService : ICategoriesServices
    {
        private readonly IGenericRepository<Category?> _categoryRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category?> categoryRepository, IImageService imageService, IMapper mapper)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddCategory(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentNullException("Category name is required");
            }

            var category = _mapper.Map<Category>(dto);

            if (dto.ImageFile != null)
            {
                var uploadResult = await _imageService.UploadImageAsync(dto.ImageFile);
                category.ImageUrl = uploadResult.Url;
            }

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentNullException("Category name is required");
            }

            var category = await _categoryRepository.GetByIdAsync(dto.Id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            _mapper.Map(dto, category);

            if (dto.ImageFile != null)
            {
                var uploadResult = await _imageService.UploadImageAsync(dto.ImageFile);
                category.ImageUrl = uploadResult.Url;
            }

            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveAsync();
            return true;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            return _mapper.Map<CategoryDto?>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        //public async Task<IEnumerable<CategoryDto>> FindAsync(Expression<Func<Category?, bool>> func)
        //{
        //    var categories = await _categoryRepository.FindAsync(func , includeword:null);
        //    return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        //}

        public async Task<Category?> DetermineCategoryAsync(string categoryName)
        {
            var existingCategories = await _categoryRepository.FindAsync(x => x.Name == categoryName , includeword:null);
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
}
