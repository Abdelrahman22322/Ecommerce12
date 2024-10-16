using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "UserPolicy")]
public class WishlistItemController : ControllerBase
{
    private readonly IWishlistItemService _wishlistItemService;
    private readonly ILogger<WishlistItemController> _logger;

    public WishlistItemController(IWishlistItemService wishlistItemService, ILogger<WishlistItemController> logger)
    {
        _wishlistItemService = wishlistItemService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddWishlistItem([FromBody] AddWishlistItemDto addWishlistItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var wishlistItem = await _wishlistItemService.AddWishlistItemAsync(addWishlistItemDto);
            return Ok(wishlistItem);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "AddWishlistItem: Key not found.");
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "AddWishlistItem: Invalid operation.");
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "AddWishlistItem: Unauthorized access.");
            return Unauthorized(ex.Message);
        }
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteWishlistItem(int productId)
    {
        try
        {
            var wishlistItem = await _wishlistItemService.DeleteWishlistItemAsync(productId);
            return Ok(wishlistItem);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "DeleteWishlistItem: Key not found.");
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "DeleteWishlistItem: Unauthorized access.");
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWishlistItems()
    {
        try
        {
            var wishlistItems = await _wishlistItemService.GetAllWishlistItemsAsync();
            return Ok(wishlistItems);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "GetAllWishlistItems: Key not found.");
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "GetAllWishlistItems: Unauthorized access.");
            return Unauthorized(ex.Message);
        }
    }
}