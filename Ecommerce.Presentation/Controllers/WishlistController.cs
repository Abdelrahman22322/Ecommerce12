using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("add-wishlist")]
        public async Task<IActionResult> AddWishlist([FromBody] WishlistDTO wishlist)
        {
            try
            {
                var result = await _wishlistService.AddWishlistAsync(wishlist);
                return CreatedAtAction(nameof(GetWishlistByUserId), new { userId = result.UserId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add-wishlist-item")]
        public async Task<IActionResult> AddWishlistItem([FromBody] WishlistItemDTO wishlistItem)
        {
            try
            {
                var result = await _wishlistService.AddWishlistItemAsync(wishlistItem);
                return CreatedAtAction(nameof(GetWishlistByUserId), new { userId = wishlistItem.WishlistId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetWishlistByUserId(int userId)
        {
            var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userId);
            if (wishlist == null)
            {
                return NotFound(new { message = "Wishlist not found." });
            }
            return Ok(wishlist);
        }

        [HttpDelete("remove-wishlist-item/{wishlistItemId}")]
        public async Task<IActionResult> RemoveWishlistItem(int wishlistItemId)
        {
            try
            {
                await _wishlistService.RemoveWishlistItemAsync(wishlistItemId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}