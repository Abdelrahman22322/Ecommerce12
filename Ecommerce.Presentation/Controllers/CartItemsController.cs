using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.API.Controllers
{
   /// <summary>
   /// [Authorize(Policy = "UserPolicy")]
   /// </summary>
    
    [ApiController]
    
    [Route("api/[controller]")]
    
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }
         [Authorize(Policy = "UserPolicy")]
       // [AllowAnonymous]
        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] AddCartItemDto addCartItemDto, string token)
        {
            try
            {
                await _cartItemService.AddOrUpdateCartItemAsync(addCartItemDto , token);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto, string token)
        {
            try
            {
                var result = await _cartItemService.UpdateCartItemAsync(updateCartItemDto,token);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteCartItem(int productId ,string token)
        {
            try
            {
                int userId = GetUserIdFromToken(); // Use token to get userId
                var result = await _cartItemService.DeleteCartItemAsync(productId, token);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCartItems( string token)
        {
            try
            {
                var result = await _cartItemService.GetAllCartItemsAsync(token);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authorized.");
            }
            return int.Parse(userIdClaim.Value);
        }
    }
}
