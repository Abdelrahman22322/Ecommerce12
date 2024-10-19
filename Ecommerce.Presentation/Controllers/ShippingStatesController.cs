using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShippingStatesController : ControllerBase
{
    private readonly IShippingStateService _shippingStateService;

    public ShippingStatesController(IShippingStateService shippingStateService)
    {
        _shippingStateService = shippingStateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShippingStates()
    {
        try
        {
            var shippingStates = await _shippingStateService.GetAllShippingStatesAsync();
            return Ok(shippingStates);
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here)
            return StatusCode(500, "Internal server error" + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShippingStateById(int id)
    {
        try
        {
            var shippingState = await _shippingStateService.GetShippingStateByIdAsync(id);
            if (shippingState == null)
            {
                return NotFound();
            }
            return Ok(shippingState);
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here)
            return StatusCode(500, "Internal server error" + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddShippingState([FromBody] ShippingStateDto shippingStateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdShippingState = await _shippingStateService.AddShippingStateAsync(shippingStateDto);
            return CreatedAtAction(nameof(GetShippingStateById), new { id = createdShippingState.Id }, createdShippingState );
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here)
            return StatusCode(500, "Internal server error" + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShippingState(int id, [FromBody] ShippingStateDto shippingStateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            shippingStateDto.Id = id;
            var updatedShippingState = await _shippingStateService.UpdateShippingStateAsync(shippingStateDto);
            if (updatedShippingState == null)
            {
                return NotFound();
            }
            return Ok(updatedShippingState);
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here)
            return StatusCode(500, "Internal server error" + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShippingState(int id)
    {
        try
        {
            await _shippingStateService.DeleteShippingStateAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here)
            return StatusCode(500, "Internal server error" + ex.InnerException);
        }
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetShippingStatesByStatus(ShippingStatus status)
    {
        try
        {
            var shippingStates = await _shippingStateService.GetShippingStatesByStatusAsync(status);
            return Ok(shippingStates);
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here)
            return StatusCode(500, "Internal server error");
        }
    }
}