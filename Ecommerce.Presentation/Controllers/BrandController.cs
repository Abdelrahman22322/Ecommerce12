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
    public class BrandController : ControllerBase
    {
        private readonly IBrandServices _brandServices;

        public BrandController(IBrandServices brandServices)
        {
            _brandServices = brandServices;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBrandDto dto)
        {
            try
            {
                await _brandServices.AddBrand(dto);
                return Ok(new { Message = "Brand created successfully" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateBrandDto dto)
        {
            try
            {
                await _brandServices.UpdateAsync(dto);
                return Ok(new { Message = "Brand updated successfully" });
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
                await _brandServices.DeleteAsync(id);
                return Ok(new { Message = "Brand deleted successfully" });
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
                var brand = await _brandServices.GetByIdAsync(id);
                return Ok(brand);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var brands = await _brandServices.GetAllAsync();
        //    return Ok(brands);
        //}
    }
}
