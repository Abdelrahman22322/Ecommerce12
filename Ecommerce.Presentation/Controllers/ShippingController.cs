using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
    private readonly IShippingService _shippingService;

    public ShippingController(IShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipping([FromBody] ShippingDto shippingDto)
    {
        try
        {
            var result = await _shippingService.CreateShippingAsync(shippingDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the shipping.", Details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShippingById(int id)
    {
        try
        {
            var result = await _shippingService.GetShippingByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { Message = "Shipping not found." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the shipping.", Details = ex.Message });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetShippingsByStatus(ShippingStatus status)
    {
        try
        {
            var result = await _shippingService.GetShippingsByStatusAsync(status);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving shippings by status.", Details = ex.Message });
        }
    }

    [HttpGet("tracking/{trackingCode}")]
    public async Task<IActionResult> GetShippingByTrackingCode(string trackingCode)
    {
        try
        {
            var result = await _shippingService.GetShippingByTrackingCodeAsync(trackingCode);
            if (result == null)
            {
                return NotFound(new { Message = "Shipping not found with the provided tracking code." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the shipping by tracking code.", Details = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateShippingStatus(int id, [FromBody] ShippingStatus newStatus)
    {
        try
        {
            await _shippingService.UpdateShippingStatusAsync(id, newStatus);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the shipping status.", Details = ex.Message });
        }
    }

    [HttpPut("{id}/method")]
    public async Task<IActionResult> AssignShippingMethod(int id, [FromBody] ShippingMethod method, decimal cost)
    {
        try
        {
            await _shippingService.AssignShippingMethodAsync(id, method, cost);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while assigning the shipping method.", Details = ex.Message });
        }
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignShippingToOrder(int orderId, int shipperId, [FromBody] ShippingMethod method, decimal cost, string trackingCode)
    {
        try
        {
            await _shippingService.AssignShippingToOrderAsync(orderId, shipperId, method, cost, trackingCode);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while assigning the shipping to the order.", Details = ex.Message });
        }
    }

    [HttpGet("tracking/{trackingCode}/status")]
    public async Task<IActionResult> GetShippingStatusByTrackingCode(string trackingCode)
    {
        try
        {
            var result = await _shippingService.GetShippingStatusByTrackingCodeAsync(trackingCode);
            if (result == null)
            {
                return NotFound(new { Message = "Shipping status not found with the provided tracking code." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the shipping status by tracking code.", Details = ex.Message });
        }
    }

    [HttpPut("cost")]
    public IActionResult AssignShippingCost([FromBody] ShippingMethod method, decimal cost)
    {
        try
        {
            _shippingService.AssignShippingCost(method, cost);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while assigning the shipping cost.", Details = ex.Message });
        }
    }

    [HttpGet("price/{method}")]
    public IActionResult GetShippingPrice(ShippingMethod method)
    {
        try
        {
            var result = _shippingService.GetShippingPrice(method);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the shipping price.", Details = ex.Message });
        }
    }

    [HttpPut("disable/{method}")]
    public IActionResult DisableShippingMethod(ShippingMethod method)
    {
        try
        {
            _shippingService.DisableShippingMethod(method);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while disabling the shipping method.", Details = ex.Message });
        }
    }

    [HttpPut("enable/{method}")]
    public IActionResult EnableShippingMethod(ShippingMethod method)
    {
        try
        {
            _shippingService.EnableShippingMethod(method);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while enabling the shipping method.", Details = ex.Message });
        }
    }
}