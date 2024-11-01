﻿using Microsoft.AspNetCore.Mvc;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.ServicesContracts;
using System.Linq.Expressions;
using System.Net;
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
                return NotFound(ex.InnerException);
            }
        }

        // POST: api/Discount/Find
        //[HttpPost("Find")]
        //public async Task<IActionResult> Find([FromBody] Expression<Func<Discount, bool>> func)
        //{
        //    var discounts = await _discountService.FindAsync(func);
        //    return Ok(discounts);
        //}
        // POST: api/Discount/AddByCategory
        [HttpPost("AddByCategory")]
        public async Task<IActionResult> AddDiscountByCategory([FromBody] DiscountByCategoryRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Category) || request.Discount == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                await _discountService.AddDiscountByCategoryAsync(request.Category, request.Discount);
                return StatusCode((int)HttpStatusCode.Created, "Discount added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error adding discount: {ex.Message}");
            }
        }

        // POST: api/Discount/AddByBrand
        [HttpPost("AddByBrand")]
        public async Task<IActionResult> AddDiscountByBrand([FromBody] DiscountByBrandRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Brand) || request.Discount == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                await _discountService.AddDiscountByBrandAsync(request.Brand, request.Discount);
                return StatusCode((int)HttpStatusCode.Created, "Discount added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error adding discount: {ex.Message}");
            }
        }
    }

    public class DiscountByCategoryRequest
    {
        public string Category { get; set; }
        public DiscountByDTO Discount { get; set; }
    }

    public class DiscountByBrandRequest
    {
        public string Brand { get; set; }
        public DiscountByDTO Discount { get; set; }
    }
}
