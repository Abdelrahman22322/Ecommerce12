using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesServices _categoryService;

        public CategoryController(ICategoriesServices categoryService)
        {
            _categoryService = categoryService;
        }

        // POST: api/Category
        [HttpPost("Add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            if (category == null || string.IsNullOrEmpty(category.Name))
            {
                return BadRequest("Category name is required.");
            }

            await _categoryService.AddCategory(category);
            return Ok(category);
        }

        // GET: api/Category
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto? category)
        {
            if (category == null  || string.IsNullOrEmpty(category.Name))
            {
                return BadRequest("Invalid category data.");
            }

            var existingCategory = await _categoryService.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _categoryService.UpdateAsync(category);
            return Ok(category);
        }


    }
}
