using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-cart")]
        public async Task<IActionResult> AddCart([FromBody] CartDTO cart)
        {
            try
            {
                var result = await _cartService.AddCartAsync(cart);
                return CreatedAtAction(nameof(GetCartByUserId), new { userId = result.UserId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add-cart-item")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemDTO cartItem)
        {
            try
            {
                var result = await _cartService.AddCartItemAsync(cartItem);
                return CreatedAtAction(nameof(GetCartByUserId), new { userId = cartItem.CartId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound(new { message = "Cart not found." });
            }
            return Ok(cart);
        }

        [HttpDelete("remove-cart-item/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            try
            {
                await _cartService.RemoveCartItemAsync(cartItemId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}