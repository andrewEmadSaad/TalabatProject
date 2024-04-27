﻿using System.Collections.Generic;
using System;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Apis.Dtos
{
    public class OrderToReturnDto
    {
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; } 

        public Address ShippingAddress { get; set; }

        //public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [One]
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } 

        public decimal SubTotal { get; set; }

        public string PaymentIntendId { get; set; }

        public decimal Total { get; set; }
    }
}
