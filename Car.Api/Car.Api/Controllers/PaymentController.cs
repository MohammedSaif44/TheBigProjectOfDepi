using CarRental.App.DTOs;
using CarRental.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace CarRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly IConfiguration _config;

        public PaymentController(PaymentService paymentService, IConfiguration config)
        {
            _paymentService = paymentService;
            _config = config;
        }

        [Authorize(Roles = "Customer ,Admin")]
        [HttpPost("create-session")]
        public async Task<IActionResult> CreateSession(CreatePaymentDto dto)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                     ?? User.Identity?.Name
                     ?? "customer@test.com";

            var result = await _paymentService.CreateCheckoutSessionAsync(dto, email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetBySessionId(string sessionId)
        {
            var result = await _paymentService.GetBySessionIdAsync(sessionId);

            if (result == null)
                return NotFound(new { message = "Payment session not found." });

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _paymentService.GetAllAsync();
            return Ok(list);
        }
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            Request.EnableBuffering();
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];
            var webhookSecret = _config["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;

                    if (session == null)
                        return BadRequest("Invalid session data.");

                    await _paymentService.HandleCheckoutCompletedAsync(session);
                }


                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("WEBHOOK ERROR: " + ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
