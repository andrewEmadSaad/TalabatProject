﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications
{
    public class OrderByPaymentIntentIdSpecification:BaseSpecification<Order>
    {

        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
            :base(O=>O.PaymentIntendId==paymentIntentId)
        {

        }
    }
}
