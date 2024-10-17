using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesServices _categoryServices;

        public CategoryController(ICategoriesServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto dto)
        {
            try
            {
                await _categoryServices.AddCategory(dto);
                return Ok(new { Message = "Category created successfully" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateCategoryDto dto)
        {
            try
            {
                await _categoryServices.UpdateAsync(dto);
                return Ok(new { Message = "Category updated successfully" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryServices.DeleteAsync(id);
                return Ok(new { Message = "Category deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryServices.GetByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryServices.GetAllAsync();
            return Ok(categories);
        }



    }
}
