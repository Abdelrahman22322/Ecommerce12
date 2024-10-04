using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;
    private readonly IMapper _mapper;

    public CheckoutController(ICheckoutService checkoutService, IMapper mapper)
    {
        _checkoutService = checkoutService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Checkout([FromBody] CheckoutDto checkoutDto ,int userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _checkoutService.CheckoutAsync(checkoutDto, userId);
            return Ok(new { message = "Checkout completed successfully." });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }
}