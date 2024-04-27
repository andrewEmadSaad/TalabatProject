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

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            ) 
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderRepo = orderRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {

            // 1. Get Basket Form Basket Repo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo

            var OrderItems=new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);

                var OrderItem=new OrderItem(productItemOrder,product.Price,item.Quantity);
           
                OrderItems.Add(OrderItem);
            }

            // 3. Calculate SubTotal

            var subTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4. Get DeliveryMethod From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5. Create Order

            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var exisitingOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (exisitingOrder!=null)
            {
                _unitOfWork.Repository<Order>().Delete(exisitingOrder);
                await _paymentService.CreateOrUpdatePaymentIntentAsync(basket.Id);
            }


            var order = new Order(buyerEmail,ShippingAddress,deliveryMethod,OrderItems,subTotal,basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().CreateAsync(order);

            // 6. Save To DataBase [TODO]
            var result= await _unitOfWork.Complete();
            if(result<=0) return null;

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliveryMethods;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecification(buyerEmail,orderId);
            var order=await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
       
            return orders;
        }
    }
}
