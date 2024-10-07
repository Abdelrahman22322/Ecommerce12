using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Stripe;

[ApiController]
[Route("api/[controller]")]
public class StripeWebhookController : ControllerBase
{
    private readonly string _webhookSecret;
    private readonly IOrderService _orderService;

    public StripeWebhookController(IConfiguration configuration, IOrderService orderService)
    {
        _webhookSecret = configuration["Stripe:WebhookSecret"] ?? throw new ArgumentNullException(nameof(configuration), "Stripe webhook secret is not configured.");
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeSignature = Request.Headers["Stripe-Signature"];

        // Log the incoming request for debugging
        Console.WriteLine("Received Stripe webhook:");
        Console.WriteLine($"Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
        Console.WriteLine($"Body: {json}");

        if (string.IsNullOrEmpty(stripeSignature))
        {
            Console.WriteLine("Missing Stripe-Signature header");
            return BadRequest("Missing Stripe-Signature header");
        }

        if (string.IsNullOrEmpty(_webhookSecret))
        {
            Console.WriteLine("Webhook secret not configured");
            return StatusCode(500, "Webhook secret not configured");
        }

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);

            switch (stripeEvent.Type)
            {
                case Events.ChargeSucceeded:
                    var charge = stripeEvent.Data.Object as Charge;
                    if (charge != null)
                    {
                        // Handle the charge succeeded event
                        Console.WriteLine($"Charge succeeded for charge ID: {charge.Id}");

                        // Complete the order using the charge ID
                        var orderDto = await _orderService.CompleteOrderByChargeIdAsync(charge.Id);
                        Console.WriteLine($"Order completed with ID: {orderDto.Id}");
                    }
                    else
                    {
                        Console.WriteLine("Charge object is null");
                        return BadRequest("Charge object is null");
                    }
                    break;
                case Events.PaymentIntentSucceeded:
                    Console.WriteLine("PaymentIntent succeeded");
                    break;
                case Events.PaymentIntentPaymentFailed:
                    Console.WriteLine("PaymentIntent failed");
                    break;
                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Stripe exception: {e.Message}");
            return BadRequest($"Stripe exception: {e.Message}");
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Argument null exception: {e.Message}");
            return BadRequest($"Argument null exception: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"General exception: {e.Message}");
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

}
