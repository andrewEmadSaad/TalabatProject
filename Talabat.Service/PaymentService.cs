using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _Configuration;
        private readonly IBasketRepository _BasketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork
            )
        {

            _Configuration = configuration;
            _BasketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            //Connect To Stripe 
            StripeConfiguration.ApiKey = _Configuration["StripeSettings:SecretKey"];

            //Get Basket By Basket Id
            var basket = await _BasketRepository.GetBasketAsync(basketId);

            if (basket == null) return null;

            var shippingPrice = 0m;

            // Get DeliveryMethodPrice
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;

                basket.ShippingPrice = shippingPrice;
            }

            // Check Basket Items Prices
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            //check if I have Existed Payment Intint
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create Payment Intint in Stripe
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice*100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret= intent.ClientSecret;
            }
            else
            {
                // Update Payment Intent in Stripe

                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice*100,

                };
                await service.UpdateAsync(basket.PaymentIntentId, options);

            }

            //Update Basket in Redis Database After Changes 
            await _BasketRepository.UpdateBasketAsync(basket);
           
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentSucceededOrFailed(string paymentIntentId, bool IsSuccess)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (IsSuccess)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFaild;

            _unitOfWork.Repository<Order>().Update(order);
            
            await _unitOfWork.Complete();

            return order;

        }
    }
}
