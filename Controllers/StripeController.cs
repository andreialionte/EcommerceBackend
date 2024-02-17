using Ecommerce2.Data;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Ecommerce2.Controllers
{
    public class StripeController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;

        public StripeController(IConfiguration configuration)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
        }

        //start the payment(initialize it for Stripe.Js)
        [HttpPost("InitiatePayment")]
        public IActionResult InitiatePayment(int orderId)
        {
            string stripeSecretKey = _config.GetSection("Stripe:SecretKey").Value;
            StripeConfiguration.ApiKey = stripeSecretKey;

            Order? orderDb = _ef.orders.Where(o => o.OrderId == orderId).FirstOrDefault();
            if (orderDb == null)
            {
                throw new Exception($"Order with id {orderId} not found!");
            }

            decimal totalAmount = orderDb.TotalAmount;

            /*            Console.WriteLine($"Total Amount: {totalAmount}");*/

            if (totalAmount <= 0)
            {
                throw new Exception("Total amount must be greater than 0.");
            }

            /*            Console.WriteLine($"Stripe API Key: {stripeSecretKey}");*/

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(totalAmount * 100), // Convert to cents
                Currency = "RON",
                Description = $"Payment for the Order {orderId}"
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return Ok(new { ClientSecret = paymentIntent.ClientSecret });
        }



    }
}