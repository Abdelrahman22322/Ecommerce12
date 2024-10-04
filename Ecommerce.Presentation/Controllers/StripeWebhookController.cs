using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

[ApiController]
[Route("api/[controller]")]
public class StripeWebhookController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly string _webhookSecret;

    public StripeWebhookController(IOrderService orderService, IConfiguration configuration)
    {
        _orderService = orderService;
        _webhookSecret = configuration["Stripe:WebhookSecret"]; // Ensure this is set in your configuration
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeSignature = Request.Headers["Stripe-Signature"];

        if (string.IsNullOrEmpty(stripeSignature))
        {
            return BadRequest("Missing Stripe-Signature header.");
        }

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                var orderId = int.Parse(session!.ClientReferenceId);

                await _orderService.ChangeOrderStatusAsync(orderId, OrderState.Paid);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest($"Stripe exception: {e.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
}