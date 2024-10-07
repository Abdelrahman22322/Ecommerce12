using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
    {
        try
        {
            var result = await _orderService.CreateOrderAsync(orderCreateDto);
            return Ok(new { CheckoutUrl = result.CheckoutUrl });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the order.", Details = ex.Message });
        }
    }

    //[HttpPost("complete")]
    //public async Task<IActionResult> CompleteOrder([FromBody] string sessionId)
    //{
    //    try
    //    {
    //        var result = await _orderService.CompleteOrderAsync(sessionId);
    //        return Ok(result);
    //    }
    //    catch (InvalidOperationException ex)
    //    {
    //        return BadRequest(new { Message = ex.Message });
    //    }
    //    catch (KeyNotFoundException ex)
    //    {
    //        return NotFound(new { Message = ex.Message });
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, new { Message = "An error occurred while completing the order.", Details = ex.Message });
    //    }
    //}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        try
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { Message = "Order not found." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the order.", Details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving orders.", Details = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] OrderDto orderDto)
    {
        try
        {
            await _orderService.UpdateOrderAsync(orderDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the order.", Details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the order.", Details = ex.Message });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetOrdersByStatus(OrderState status)
    {
        try
        {
            var result = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving orders by status.", Details = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeOrderStatus(int id, [FromBody] OrderState newStatus)
    {
        try
        {
            await _orderService.ChangeOrderStatusAsync(id, newStatus);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while changing the order status.", Details = ex.Message });
        }
    }
}
