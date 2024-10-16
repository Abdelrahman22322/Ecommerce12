using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ShippingMethodsController : ControllerBase
{
    private readonly IShippingMethodService _shippingMethodService;
    private readonly ILogger<ShippingMethodsController> _logger;

    public ShippingMethodsController(IShippingMethodService shippingMethodService, ILogger<ShippingMethodsController> logger)
    {
        _shippingMethodService = shippingMethodService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShippingMethods()
    {
        try
        {
            var shippingMethods = await _shippingMethodService.GetAllShippingMethodsAsync();
            return Ok(shippingMethods);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all shipping methods");
            return StatusCode(500, new { Message = "An error occurred while retrieving all shipping methods.", Details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShippingMethodById(int id)
    {
        try
        {
            var shippingMethod = await _shippingMethodService.GetShippingMethodByIdAsync(id);
            if (shippingMethod == null)
            {
                return NotFound(new { Message = "Shipping method not found." });
            }
            return Ok(shippingMethod);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the shipping method with ID {Id}", id);
            return StatusCode(500, new { Message = "An error occurred while retrieving the shipping method.", Details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddShippingMethod([FromBody] ShippingMethodDto shippingMethodDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdShippingMethod = await _shippingMethodService.AddShippingMethodAsync(shippingMethodDto);
            return CreatedAtAction(nameof(GetShippingMethodById), new { id = createdShippingMethod.Id }, createdShippingMethod);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new shipping method");
            return StatusCode(500, new { Message = "An error occurred while adding the shipping method.", Details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShippingMethod(int id, [FromBody] ShippingMethodDto shippingMethodDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            shippingMethodDto.Id = id;
            var updatedShippingMethod = await _shippingMethodService.UpdateShippingMethodAsync(shippingMethodDto);
            if (updatedShippingMethod == null)
            {
                return NotFound(new { Message = "Shipping method not found." });
            }
            return Ok(updatedShippingMethod);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the shipping method with ID {Id}", id);
            return StatusCode(500, new { Message = "An error occurred while updating the shipping method.", Details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShippingMethod(int id)
    {
        try
        {
            await _shippingMethodService.DeleteShippingMethodAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the shipping method with ID {Id}", id);
            return StatusCode(500, new { Message = "An error occurred while deleting the shipping method.", Details = ex.Message });
        }
    }

    [HttpGet("cost-range")]
    public async Task<IActionResult> GetShippingMethodsByCostRange([FromQuery] decimal minCost, [FromQuery] decimal maxCost)
    {
        try
        {
            var shippingMethods = await _shippingMethodService.GetShippingMethodsByCostRangeAsync(minCost, maxCost);
            return Ok(shippingMethods);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving shipping methods by cost range");
            return StatusCode(500, new { Message = "An error occurred while retrieving shipping methods by cost range.", Details = ex.Message });
        }
    }

    [HttpGet("cheapest")]
    public async Task<IActionResult> GetCheapestShippingMethod()
    {
        try
        {
            var shippingMethod = await _shippingMethodService.GetCheapestShippingMethodAsync();
            return Ok(shippingMethod);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the cheapest shipping method");
            return StatusCode(500, new { Message = "An error occurred while retrieving the cheapest shipping method.", Details = ex.Message });
        }
    }

    [HttpGet("{id}/price")]
    public async Task<IActionResult> GetShippingPrice(int id)
    {
        try
        {
            var price = await _shippingMethodService.GetShippingPriceAsync(id);
            return Ok(price);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the price for the shipping method with ID {Id}.", id);
            return StatusCode(500, new { Message = "An error occurred while retrieving the shipping price.", Details = ex.Message });
        }
    }
}
