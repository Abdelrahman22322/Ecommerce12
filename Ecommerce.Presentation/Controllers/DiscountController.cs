using Microsoft.AspNetCore.Mvc;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.ServicesContracts;
using System.Linq.Expressions;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: api/Discount
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discounts = await _discountService.GetAllAsync();
            return Ok(discounts);
        }

        // GET: api/Discount/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var discount = await _discountService.GetByIdAsync(id);
                return Ok(discount);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Discount
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DiscountDTO discount)
        {
            if (discount == null)
            {
                return BadRequest("Discount is null.");
            }

            await _discountService.AddAsync(discount);

            return Ok(discount); // Return the newly created Discount
        }

        // PUT: api/Discount/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscountDTO discount)
        {
            if (discount == null)
            {
                return BadRequest("ID mismatch or discount is null.");
            }

            try
            {
                await _discountService.UpdateAsync(id, discount);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Discount/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _discountService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Discount/Find
        //[HttpPost("Find")]
        //public async Task<IActionResult> Find([FromBody] Expression<Func<Discount, bool>> func)
        //{
        //    var discounts = await _discountService.FindAsync(func);
        //    return Ok(discounts);
        //}
    }
}
