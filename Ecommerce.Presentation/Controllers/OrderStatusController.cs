using AutoMapper;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderStatusController : ControllerBase
{
    private readonly IOrderStateService _orderStateService;
    private readonly IMapper _mapper;

    public OrderStatusController(IOrderStateService orderStateService, IMapper mapper)
    {
        _orderStateService = orderStateService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderStatusById(int id)
    {
        try
        {
            var orderStatus = await _orderStateService.GetOrderStateByIdAsync(id);
            if (orderStatus == null)
            {
                return NotFound(new { Message = "Order status not found." });
            }
            return Ok(orderStatus);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the order status.", Details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrderStatuses()
    {
        try
        {
            var orderStatuses = await _orderStateService.GetAllOrderStatesAsync();
            return Ok(orderStatuses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the order statuses.", Details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddOrderStatus([FromBody] OrderStatusDto orderStatusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdOrderStatus = await _orderStateService.AddOrderStateAsync(orderStatusDto);
            return CreatedAtAction(nameof(GetOrderStatusById), new { id = createdOrderStatus.Id }, createdOrderStatus);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while adding the order status.", Details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusDto orderStatusDto)
    {
        if (id != orderStatusDto.Id)
        {
            return BadRequest(new { Message = "ID mismatch." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedOrderStatus = await _orderStateService.UpdateOrderStateAsync(orderStatusDto);
            return Ok(updatedOrderStatus);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the order status.", Details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderStatus(int id)
    {
        try
        {
            await _orderStateService.DeleteOrderStateAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the order status.", Details = ex.Message });
        }
    }
}