using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System;
using System.Threading.Tasks;
using Talabat.Apis.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;
using Talabat.Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Talabat.Apis.Controllers
{
    //[Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        private const string _whSecret = "whsec_ce1d5dc706feb1990d30c64bf07325755091e6cece3edd8b806c65dd0407f059";
        public PaymentsController(
            IPaymentService paymentService,
            ILogger<PaymentsController> logger
            )
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        //[Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);

            if (basket == null) return BadRequest(new ApiResponse(400, "A Problem With Your Basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _whSecret);

            PaymentIntent intent;
            Order order;

            // Handle the event
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    intent=(PaymentIntent)stripeEvent.Data.Object;
                    order = await _paymentService.UpdatePaymentIntentSucceededOrFailed(intent.Id, true);
                    _logger.LogInformation("Payment Succeeded");

                    break;
                case Events.PaymentIntentPaymentFailed:
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    order = await _paymentService.UpdatePaymentIntentSucceededOrFailed(intent.Id, true);
                    _logger.LogInformation("Payment Failed");


                    break;
                default:
                    break;
            }


           return new EmptyResult();

        }
    }
}
