using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.Apis.Dtos;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Apis.Helpers
{
    public class OrderItemPictureResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureResolver(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";

            return null;
        }
    }
}
