using System.Collections.Generic;
using Talabat.Core.Entities;

namespace Talabat.Apis.Dtos
{
    public class CustomerBasketDto
    {

        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
    }
}
