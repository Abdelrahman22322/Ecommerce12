using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ShipperController : ControllerBase
{
    private readonly IShipperService _shipperService;
    private readonly ILogger<ShipperController> _logger;

    public ShipperController(IShipperService shipperService, ILogger<ShipperController> logger)
    {
        _shipperService = shipperService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipper([FromBody] ShipperDto shipperDto)
    {
        try
        {
            var result = await _shipperService.CreateShipperAsync(shipperDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the shipper.");
            return StatusCode(500, new { Message = "An error occurred while creating the shipper.", Details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShipperById(int id)
    {
        try
        {
            var result = await _shipperService.GetShipperByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { Message = "Shipper not found." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the shipper with ID {id}.");
            return StatusCode(500, new { Message = "An error occurred while retrieving the shipper.", Details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShippers()
    {
        try
        {
            var result = await _shipperService.GetAllShippersAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving shippers.");
            return StatusCode(500, new { Message = "An error occurred while retrieving shippers.", Details = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateShipper([FromBody] ShipperDto shipperDto)
    {
        try
        {
            await _shipperService.UpdateShipperAsync(shipperDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the shipper with ID {shipperDto.Id}.");
            return StatusCode(500, new { Message = "An error occurred while updating the shipper.", Details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShipper(int id)
    {
        try
        {
            await _shipperService.DeleteShipperAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"Shipper with ID {id} not found.");
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the shipper with ID {id}.");
            return StatusCode(500, new { Message = "An error occurred while deleting the shipper.", Details = ex.Message });
        }
    }

    [HttpGet("assign-order")]
    public async Task<IActionResult> AssignOrderToLeastAssignedShipper()
    {
        try
        {
            var result = await _shipperService.AssignOrderToLeastAssignedShipper();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while assigning the order to the least assigned shipper.");
            return StatusCode(500, new { Message = "An error occurred while assigning the order to the least assigned shipper.", Details = ex.Message });
        }
    }
}
