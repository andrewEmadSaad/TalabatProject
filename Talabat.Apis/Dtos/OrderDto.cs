using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Apis.Dtos
{
    public class OrderDto
    {
        public string basketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto SheppingAddress { get; set; }
    }
}
